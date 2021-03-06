﻿using NTMiner.Core.Gpus;
using NTMiner.Core.Redis;
using System.Collections.Generic;
using System.Linq;

namespace NTMiner.Core.Impl {
    public class GpuNameSet : IGpuNameSet {
        private readonly Dictionary<GpuName, int> _gpuNameCountDic = new Dictionary<GpuName, int>();
        // 该集合由人工维护，这里的GpuName是由人脑提取的显卡的特征名，能覆盖每一张显卡当出现未覆盖的显卡事件时会有人工即时补漏
        private readonly HashSet<GpuName> _gpuNameSet = new HashSet<GpuName>();
        public bool IsReadied {
            get; private set;
        }

        private readonly IGpuNameRedis _gpuNameRedis;
        public GpuNameSet(IGpuNameRedis gpuNameRedis) {
            _gpuNameRedis = gpuNameRedis;
            gpuNameRedis.GetAllAsync().ContinueWith(t => {
                foreach (var item in t.Result) {
                    _gpuNameSet.Add(item);
                }
                IsReadied = true;
            });
            VirtualRoot.AddEventPath<ClientSetInitedEvent>("矿机列表初始化后计算显卡名称集合", LogEnum.DevConsole, action: message => {
                Init();
            }, this.GetType());
            VirtualRoot.AddEventPath<Per10MinuteEvent>("周期刷新显卡名称集合", LogEnum.DevConsole, action: message => {
                Init();
            }, this.GetType());
        }

        private void Init() {
            _gpuNameCountDic.Clear();
            foreach (var clientData in WebApiRoot.ClientDataSet.AsEnumerable()) {
                foreach (var gpuSpeedData in clientData.GpuTable) {
                    AddCount(clientData.GpuType, gpuSpeedData.Name, gpuSpeedData.TotalMemory);
                }
            }
        }

        public void AddCount(GpuType gpuType, string gpuName, ulong gpuTotalMemory) {
            if (gpuType == GpuType.Empty || string.IsNullOrEmpty(gpuName) || !GpuName.IsValidTotalMemory(gpuTotalMemory)) {
                return;
            }
            GpuName key = new GpuName {
                TotalMemory = gpuTotalMemory,
                Name = gpuName,
                GpuType = gpuType
            };
            if (_gpuNameCountDic.TryGetValue(key, out int count)) {
                _gpuNameCountDic[key] = count + 1;
            }
            else {
                _gpuNameCountDic.Add(key, 1);
            }
        }

        public void Set(GpuName gpuName) {
            if (!IsReadied) {
                return;
            }
            if (gpuName == null || !gpuName.IsValid()) {
                return;
            }
            _gpuNameSet.Add(gpuName);
            _gpuNameRedis.SetAsync(gpuName);
        }

        public void Remove(GpuName gpuName) {
            if (!IsReadied) {
                return;
            }
            if (gpuName == null || !gpuName.IsValid()) {
                return;
            }
            _gpuNameSet.Remove(gpuName);
            _gpuNameRedis.DeleteAsync(gpuName);
        }

        public List<GpuNameCount> QueryGpuNameCounts(QueryGpuNameCountsRequest query, out int total) {
            List<KeyValuePair<GpuName, int>> list = new List<KeyValuePair<GpuName, int>>();
            bool isFilterByKeyword = !string.IsNullOrEmpty(query.Keyword);
            if (isFilterByKeyword) {
                foreach (var item in _gpuNameCountDic.OrderBy(a => a.Key.Name)) {
                    if (item.Key.Name.Contains(query.Keyword)) {
                        list.Add(item);
                    }
                }
            }
            else {
                list.AddRange(_gpuNameCountDic);
            }
            total = list.Count;
            return list.Take(query).Select(a => new GpuNameCount {
                Name = a.Key.Name,
                Count = a.Value,
                GpuType = a.Key.GpuType,
                TotalMemory = a.Key.TotalMemory
            }).ToList();
        }

        public List<GpuName> GetAllGpuNames() {
            if (!IsReadied) {
                return new List<GpuName>();
            }
            return _gpuNameSet.ToList();
        }
    }
}
