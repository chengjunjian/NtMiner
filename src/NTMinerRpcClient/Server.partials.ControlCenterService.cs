﻿using NTMiner.MinerServer;
using NTMiner.Profile;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTMiner {
    public static partial class Server {
        public class ControlCenterServiceFace {
            public static readonly ControlCenterServiceFace Instance = new ControlCenterServiceFace();

            private ControlCenterServiceFace() {
            }

            #region LoginAsync
            public void LoginAsync(string loginName, string password, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        LoginControlCenterRequest request = new LoginControlCenterRequest {
                            LoginName = loginName
                        };
                        request.SignIt(password);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "LoginControlCenter", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region GetUsers
            /// <summary>
            /// 同步方法
            /// </summary>
            /// <param name="messageId"></param>
            /// <returns></returns>
            public GetUsersResponse GetUsers(Guid messageId) {
                try {
                    UsersRequest request = new UsersRequest {
                        LoginName = LoginName,
                        MessageId = Guid.NewGuid()
                    };
                    request.SignIt(PasswordSha1);
                    GetUsersResponse response = Request<GetUsersResponse>("ControlCenter", "Users", request);
                    return response;
                }
                catch (Exception e) {
                    Logger.ErrorDebugLine(e.Message, e);
                    return null;
                }
            }
            #endregion

            #region AddUserAsync
            public void AddUserAsync(UserData userData, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        AddUserRequest request = new AddUserRequest() {
                            LoginName = LoginName,
                            Data = userData
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "AddUser", request);
                        callback?.Invoke(response);
                    }
                    catch {
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region UpdateUserAsync
            public void UpdateUserAsync(UserData userData, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        UpdateUserRequest request = new UpdateUserRequest() {
                            LoginName = LoginName,
                            Data = userData
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "UpdateUser", request);
                        callback?.Invoke(response);
                    }
                    catch {
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region RemoveUserAsync
            public void RemoveUserAsync(string loginName, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        RemoveUserRequest request = new RemoveUserRequest() {
                            LoginName = LoginName,
                            Data = loginName
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "RemoveUser", request);
                        callback?.Invoke(response);
                    }
                    catch {
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region LoadClientsAsync
            public void LoadClientsAsync(List<Guid> clientIds, Action<LoadClientsResponse> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        LoadClientsRequest request = new LoadClientsRequest {
                            LoginName = LoginName,
                            ClientIds = clientIds
                        };
                        request.SignIt(PasswordSha1);
                        LoadClientsResponse response = Request<LoadClientsResponse>("ControlCenter", "LoadClients", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region GetLatestSnapshotsAsync
            public void GetLatestSnapshotsAsync(
                int limit,
                List<string> coinCodes,
                Action<GetCoinSnapshotsResponse> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        GetCoinSnapshotsRequest request = new GetCoinSnapshotsRequest {
                            LoginName = LoginName,
                            Limit = limit
                        };
                        request.SignIt(PasswordSha1);
                        GetCoinSnapshotsResponse response = Request<GetCoinSnapshotsResponse>("ControlCenter", "LatestSnapshots", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region LoadClientAsync
            public void LoadClientAsync(Guid clientId, Action<LoadClientResponse> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        LoadClientRequest request = new LoadClientRequest {
                            LoginName = LoginName,
                            ClientId = clientId
                        };
                        request.SignIt(PasswordSha1);
                        LoadClientResponse response = Request<LoadClientResponse>("ControlCenter", "LoadClient", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region QueryClientsAsync
            public void QueryClientsAsync(
                int pageIndex,
                int pageSize,
                DateTime? timeLimit,
                Guid? groupId,
                Guid? workId,
                string minerIp,
                string minerName,
                MineStatus mineState,
                string mainCoin,
                string mainCoinPool,
                string mainCoinWallet,
                string dualCoin,
                string dualCoinPool,
                string dualCoinWallet,
                string version,
                string kernel,
                Action<QueryClientsResponse> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        var request = new QueryClientsRequest {
                            LoginName = LoginName,
                            PageIndex = pageIndex,
                            PageSize = pageSize,
                            TimeLimit = timeLimit,
                            GroupId = groupId,
                            WorkId = workId,
                            MinerIp = minerIp,
                            MinerName = minerName,
                            MineState = mineState,
                            MainCoin = mainCoin,
                            MainCoinPool = mainCoinPool,
                            MainCoinWallet = mainCoinWallet,
                            DualCoin = dualCoin,
                            DualCoinPool = dualCoinPool,
                            DualCoinWallet = dualCoinWallet,
                            Version = version,
                            Kernel = kernel
                        };
                        request.SignIt(PasswordSha1);
                        QueryClientsResponse response = Request<QueryClientsResponse>("ControlCenter", "QueryClients", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region UpdateClientAsync
            public void UpdateClientAsync(Guid clientId, string propertyName, object value, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        UpdateClientRequest request = new UpdateClientRequest {
                            LoginName = LoginName,
                            ClientId = clientId,
                            PropertyName = propertyName,
                            Value = value
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "UpdateClient", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region UpdateClientPropertiesAsync
            public void UpdateClientPropertiesAsync(Guid clientId, Dictionary<string, object> values, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        UpdateClientPropertiesRequest request = new UpdateClientPropertiesRequest {
                            LoginName = LoginName,
                            ClientId = clientId,
                            Values = values
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "UpdateClientProperties", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region GetMinerGroups
            /// <summary>
            /// 同步方法
            /// </summary>
            /// <param name="messageId"></param>
            /// <returns></returns>
            public GetMinerGroupsResponse GetMinerGroups(Guid messageId) {
                try {
                    MinerGroupsRequest request = new MinerGroupsRequest {
                        MessageId = Guid.NewGuid()
                    };
                    GetMinerGroupsResponse response = Request<GetMinerGroupsResponse>("ControlCenter", "MinerGroups", request);
                    return response;
                }
                catch (Exception e) {
                    Logger.ErrorDebugLine(e.Message, e);
                    return null;
                }
            }
            #endregion

            #region AddOrUpdateMinerGroupAsync
            public void AddOrUpdateMinerGroupAsync(MinerGroupData entity, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        entity.ModifiedOn = DateTime.Now;
                        AddOrUpdateMinerGroupRequest request = new AddOrUpdateMinerGroupRequest {
                            LoginName = LoginName,
                            Data = entity
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "AddOrUpdateMinerGroup", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region RemoveMinerGroupAsync
            public void RemoveMinerGroupAsync(Guid id, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        RemoveMinerGroupRequest request = new RemoveMinerGroupRequest() {
                            LoginName = LoginName,
                            MinerGroupId = id
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "RemoveMinerGroup", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region AddOrUpdateMineWorkAsync
            public void AddOrUpdateMineWorkAsync(MineWorkData entity, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        entity.ModifiedOn = DateTime.Now;
                        AddOrUpdateMineWorkRequest request = new AddOrUpdateMineWorkRequest {
                            LoginName = LoginName,
                            Data = entity
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "AddOrUpdateMineWork", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region RemoveMineWorkAsync
            public void RemoveMineWorkAsync(Guid id, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        RemoveMineWorkRequest request = new RemoveMineWorkRequest {
                            LoginName = LoginName,
                            MineWorkId = id
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "RemoveMineWork", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region SetMinerProfilePropertyAsync
            public void SetMinerProfilePropertyAsync(Guid workId, string propertyName, object value, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        SetMinerProfilePropertyRequest request = new SetMinerProfilePropertyRequest() {
                            LoginName = LoginName,
                            PropertyName = propertyName,
                            Value = value,
                            WorkId = workId
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "SetMinerProfileProperty", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region SetCoinProfilePropertyAsync
            public void SetCoinProfilePropertyAsync(Guid workId, Guid coinId, string propertyName, object value, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        SetCoinProfilePropertyRequest request = new SetCoinProfilePropertyRequest {
                            LoginName = LoginName,
                            CoinId = coinId,
                            WorkId = workId,
                            PropertyName = propertyName,
                            Value = value
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "SetCoinProfileProperty", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region SetPoolProfilePropertyAsync
            public void SetPoolProfilePropertyAsync(Guid workId, Guid poolId, string propertyName, object value, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        SetPoolProfilePropertyRequest request = new SetPoolProfilePropertyRequest {
                            LoginName = LoginName,
                            PoolId = poolId,
                            WorkId = workId,
                            PropertyName = propertyName,
                            Value = value
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "SetPoolProfileProperty", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region SetCoinKernelProfilePropertyAsync
            public void SetCoinKernelProfilePropertyAsync(Guid workId, Guid coinKernelId, string propertyName, object value, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        SetCoinKernelProfilePropertyRequest request = new SetCoinKernelProfilePropertyRequest {
                            LoginName = LoginName,
                            CoinKernelId = coinKernelId,
                            PropertyName = propertyName,
                            Value = value,
                            WorkId = workId
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "SetCoinKernelProfileProperty", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region GetWallets
            /// <summary>
            /// 同步方法
            /// </summary>
            /// <returns></returns>
            public GetWalletsResponse GetWallets() {
                try {
                    WalletsRequest request = new WalletsRequest {
                        MessageId = Guid.NewGuid()
                    };
                    GetWalletsResponse response = Request<GetWalletsResponse>("ControlCenter", "Wallets", request);
                    return response;
                }
                catch (Exception e) {
                    Logger.ErrorDebugLine(e.Message, e);
                    return null;
                }
            }
            #endregion

            #region AddOrUpdateWalletAsync
            public void AddOrUpdateWalletAsync(WalletData entity, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        entity.ModifiedOn = DateTime.Now;
                        AddOrUpdateWalletRequest request = new AddOrUpdateWalletRequest {
                            LoginName = LoginName,
                            Data = entity
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "AddOrUpdateWallet", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region RemoveWalletAsync
            public void RemoveWalletAsync(Guid id, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        RemoveWalletRequest request = new RemoveWalletRequest {
                            LoginName = LoginName,
                            WalletId = id
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "RemoveWallet", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region GetCalcConfigs
            /// <summary>
            /// 同步方法
            /// </summary>
            /// <returns></returns>
            public GetCalcConfigsResponse GetCalcConfigs() {
                try {
                    CalcConfigsRequest request = new CalcConfigsRequest {
                        MessageId = Guid.NewGuid()
                    };
                    GetCalcConfigsResponse response = Request<GetCalcConfigsResponse>("ControlCenter", "CalcConfigs", request);
                    return response;
                }
                catch (Exception e) {
                    Logger.ErrorDebugLine(e.Message, e);
                    return null;
                }
            }
            #endregion

            #region SaveCalcConfigsAsync
            public void SaveCalcConfigsAsync(List<CalcConfigData> configs, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        if (configs == null || configs.Count == 0) {
                            return;
                        }
                        SaveCalcConfigsRequest request = new SaveCalcConfigsRequest {
                            Data = configs,
                            LoginName = LoginName
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "SaveCalcConfigs", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region GetColumnsShows
            /// <summary>
            /// 同步方法
            /// </summary>
            /// <param name="messageId"></param>
            /// <returns></returns>
            public GetColumnsShowsResponse GetColumnsShows(Guid messageId) {
                try {
                    ColumnsShowsRequest request = new ColumnsShowsRequest {
                        MessageId = Guid.NewGuid()
                    };
                    GetColumnsShowsResponse response = Request<GetColumnsShowsResponse>("ControlCenter", "ColumnsShows", request);
                    return response;
                }
                catch (Exception e) {
                    Logger.ErrorDebugLine(e.Message, e);
                    return null;
                }
            }
            #endregion

            #region AddOrUpdateColumnsShowAsync
            public void AddOrUpdateColumnsShowAsync(ColumnsShowData entity, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        AddOrUpdateColumnsShowRequest request = new AddOrUpdateColumnsShowRequest {
                            LoginName = LoginName,
                            Data = entity
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "AddOrUpdateColumnsShow", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion

            #region RemoveColumnsShowAsync
            public void RemoveColumnsShowAsync(Guid id, Action<ResponseBase> callback) {
                Task.Factory.StartNew(() => {
                    try {
                        RemoveColumnsShowRequest request = new RemoveColumnsShowRequest() {
                            LoginName = LoginName,
                            ColumnsShowId = id
                        };
                        request.SignIt(PasswordSha1);
                        ResponseBase response = Request<ResponseBase>("ControlCenter", "RemoveColumnsShow", request);
                        callback?.Invoke(response);
                    }
                    catch (Exception e) {
                        Logger.ErrorDebugLine(e.Message, e);
                        callback?.Invoke(null);
                    }
                });
            }
            #endregion
        }
    }
}