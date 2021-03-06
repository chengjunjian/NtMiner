﻿using RabbitMQ.Client;

namespace NTMiner.Core.Mq.Senders.Impl {
    public class MinerClientMqSender : IMinerClientMqSender {
        private readonly IModel _mqChannel;
        public MinerClientMqSender(IModel mqChannel) {
            _mqChannel = mqChannel;
        }

        public void SendMinerDataAdded(string minerId) {
            if (string.IsNullOrEmpty(minerId)) {
                return;
            }
            var basicProperties = CreateBasicProperties();
            _mqChannel.BasicPublish(
                exchange: MqKeyword.NTMinerExchange,
                routingKey: MqKeyword.MinerDataAddedRoutingKey,
                basicProperties: basicProperties,
                body: MinerClientMqBodyUtil.GetMinerIdMqSendBody(minerId));
        }

        public void SendMinerDataRemoved(string minerId) {
            if (string.IsNullOrEmpty(minerId)) {
                return;
            }
            var basicProperties = CreateBasicProperties();
            _mqChannel.BasicPublish(
                exchange: MqKeyword.NTMinerExchange,
                routingKey: MqKeyword.MinerDataRemovedRoutingKey,
                basicProperties: basicProperties,
                body: MinerClientMqBodyUtil.GetMinerIdMqSendBody(minerId));
        }

        public void SendMinerSignChanged(string minerId) {
            if (string.IsNullOrEmpty(minerId)) {
                return;
            }
            var basicProperties = CreateBasicProperties();
            _mqChannel.BasicPublish(
                exchange: MqKeyword.NTMinerExchange,
                routingKey: MqKeyword.MinerSignChangedRoutingKey,
                basicProperties: basicProperties,
                body: MinerClientMqBodyUtil.GetMinerIdMqSendBody(minerId));
        }

        private IBasicProperties CreateBasicProperties() {
            var basicProperties = _mqChannel.CreateBasicProperties();
            basicProperties.Persistent = true;// 持久化的
            basicProperties.Timestamp = new AmqpTimestamp(Timestamp.GetTimestamp());
            basicProperties.AppId = ServerRoot.HostConfig.ThisServerAddress;

            return basicProperties;
        }
    }
}
