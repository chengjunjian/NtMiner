﻿using NTMiner.Core;
using NTMiner.Vms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTMiner {
    public partial class AppContext {
        public class PoolKernelViewModels : ViewModelBase {
            private readonly Dictionary<Guid, PoolKernelViewModel> _dicById = new Dictionary<Guid, PoolKernelViewModel>();
            public PoolKernelViewModels() {
                VirtualRoot.On<PoolKernelAddedEvent>("新添了矿池内核后刷新矿池内核VM内存", LogEnum.DevConsole,
                    action: (message) => {
                        if (!_dicById.ContainsKey(message.Source.GetId())) {
                            PoolViewModel poolVm;
                            if (Current.PoolVms.TryGetPoolVm(message.Source.PoolId, out poolVm)) {
                                _dicById.Add(message.Source.GetId(), new PoolKernelViewModel(message.Source));
                                poolVm.OnPropertyChanged(nameof(poolVm.PoolKernels));
                            }
                        }
                    }).AddToCollection(NTMinerRoot.Current.ContextHandlers);
                VirtualRoot.On<PoolKernelRemovedEvent>("移除了币种内核后刷新矿池内核VM内存", LogEnum.DevConsole,
                    action: (message) => {
                        if (_dicById.ContainsKey(message.Source.GetId())) {
                            var vm = _dicById[message.Source.GetId()];
                            _dicById.Remove(message.Source.GetId());
                            PoolViewModel poolVm;
                            if (Current.PoolVms.TryGetPoolVm(vm.PoolId, out poolVm)) {
                                poolVm.OnPropertyChanged(nameof(poolVm.PoolKernels));
                            }
                        }
                    }).AddToCollection(NTMinerRoot.Current.ContextHandlers);
                VirtualRoot.On<PoolKernelUpdatedEvent>("更新了矿池内核后刷新VM内存", LogEnum.DevConsole,
                    action: (message) => {
                        if (_dicById.ContainsKey(message.Source.GetId())) {
                            _dicById[message.Source.GetId()].Update(message.Source);
                        }
                    }).AddToCollection(NTMinerRoot.Current.ContextHandlers);
                Init();
            }

            private void Init() {
                foreach (IPoolKernel item in NTMinerRoot.Current.PoolKernelSet) {
                    _dicById.Add(item.GetId(), new PoolKernelViewModel(item));
                }
            }

            public List<PoolKernelViewModel> AllPoolKernels {
                get {
                    return _dicById.Values.ToList();
                }
            }
        }
    }
}
