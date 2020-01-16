﻿using NTMiner.Controllers;
using NTMiner.MinerServer;
using System;
using System.Collections.Generic;

namespace NTMiner {
    public partial class Server {
        public partial class ColumnsShowServiceFace {
            private static readonly string SControllerName = ControllerUtil.GetControllerName<IColumnsShowController>();

            public ColumnsShowServiceFace() {
            }

            #region GetColumnsShows
            // TODO:异步化
            /// <summary>
            /// 同步方法
            /// </summary>
            /// <returns></returns>
            public List<ColumnsShowData> GetColumnsShows() {
                try {
                    SignRequest request = new SignRequest {
                    };
                    DataResponse<List<ColumnsShowData>> response = RpcRoot.Post<DataResponse<List<ColumnsShowData>>>(NTMinerRegistry.GetControlCenterHost(), NTKeyword.ControlCenterPort, SControllerName, nameof(IColumnsShowController.ColumnsShows), request, request, timeout: 2000);
                    if (response != null && response.Data != null) {
                        return response.Data;
                    }
                    return new List<ColumnsShowData>();
                }
                catch (Exception e) {
                    Logger.ErrorDebugLine(e);
                    return new List<ColumnsShowData>();
                }
            }
            #endregion

            #region AddOrUpdateColumnsShowAsync
            public void AddOrUpdateColumnsShowAsync(ColumnsShowData entity, Action<ResponseBase, Exception> callback) {
                DataRequest<ColumnsShowData> request = new DataRequest<ColumnsShowData>() {
                    Data = entity
                };
                RpcRoot.PostAsync(NTMinerRegistry.GetControlCenterHost(), NTKeyword.ControlCenterPort, SControllerName, nameof(IColumnsShowController.AddOrUpdateColumnsShow), request, request, callback);
            }
            #endregion

            #region RemoveColumnsShowAsync
            public void RemoveColumnsShowAsync(Guid id, Action<ResponseBase, Exception> callback) {
                DataRequest<Guid> request = new DataRequest<Guid>() {
                    Data = id
                };
                RpcRoot.PostAsync(NTMinerRegistry.GetControlCenterHost(), NTKeyword.ControlCenterPort, SControllerName, nameof(IColumnsShowController.RemoveColumnsShow), request, request, callback);
            }
            #endregion
        }
    }
}
