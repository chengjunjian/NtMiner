﻿using LiteDB;
using NTMiner.MinerClient;
using System;
using System.Collections.Generic;

namespace NTMiner {
    public static partial class VirtualRoot {
        public static readonly bool IsGEWin10 = Environment.OSVersion.Version >= new Version(6, 2);
        public static readonly bool IsLTWin10 = Environment.OSVersion.Version < new Version(6, 2);
        public static readonly string WorkerEventDbFileFullName = System.IO.Path.Combine(MainAssemblyInfo.HomeDirFullName, "workerEvent.litedb");
        
        public static void WorkerEvent(WorkerEventChannel channel, WorkerEventType level, string content) {
            WorkerEvents.Add(channel, level, content);
        }

        public class WorkerEventSet {
            private int _lastWorkerEventId;
            private readonly string _connectionString;

            internal WorkerEventSet() {
                _connectionString = $"filename={WorkerEventDbFileFullName};journal=false";
            }

            public void Add(WorkerEventChannel channel, WorkerEventType level, string content) {
                InitOnece();
                var data = new WorkerEventData {
                    Id = 0,
                    Channel = channel,
                    Level = level,
                    Content = content,
                    EventOn = DateTime.Now
                };
                using (LiteDatabase db = new LiteDatabase(_connectionString)) {
                    var col = db.GetCollection<WorkerEventData>();
                    data.Id = col.Insert(data).AsInt32;
                    _lastWorkerEventId = data.Id;
                }
                Happened(new WorkerEvent(data));
            }

            public int LastWorkerEventId {
                get { return _lastWorkerEventId; }
            }

            private bool _isInited = false;
            private readonly object _locker = new object();

            private void InitOnece() {
                if (_isInited) {
                    return;
                }
                Init();
            }

            private void Init() {
                lock (_locker) {
                    if (!_isInited) {
                        using (LiteDatabase db = new LiteDatabase(_connectionString)) {
                            var col = db.GetCollection<WorkerEventData>();
                            _lastWorkerEventId = col.Max(a => a.Id).AsInt32;
                        }
                        _isInited = true;
                    }
                }
            }

            public IEnumerable<IWorkerEvent> GetEvents(WorkerEventChannel channel, string keyword) {
                InitOnece();
                using (LiteDatabase db = new LiteDatabase(_connectionString)) {
                    var col = db.GetCollection<WorkerEventData>();
                    if (channel != WorkerEventChannel.Undefined) {
                        if (!string.IsNullOrEmpty(keyword)) {
                            return col.Find(
                                Query.And(
                                    Query.GT("_id", _lastWorkerEventId - WorkerEventSetSliding),
                                    Query.EQ(nameof(WorkerEventData.Channel), channel.GetName()),
                                    Query.Contains(nameof(WorkerEventData.Content), keyword)));
                        }
                        else {
                            return col.Find(
                                Query.And(
                                    Query.GT("_id", _lastWorkerEventId - WorkerEventSetSliding),
                                    Query.EQ(nameof(WorkerEventData.Channel), channel.GetName())));
                        }
                    }
                    else {
                        if (!string.IsNullOrEmpty(keyword)) {
                            return col.Find(
                                Query.And(
                                    Query.GT("_id", _lastWorkerEventId - WorkerEventSetSliding),
                                    Query.Contains(nameof(WorkerEventData.Content), keyword)));
                        }
                        else {
                            return col.Find(Query.GT("_id", _lastWorkerEventId - WorkerEventSetSliding));
                        }
                    }
                }
            }
        }
    }
}
