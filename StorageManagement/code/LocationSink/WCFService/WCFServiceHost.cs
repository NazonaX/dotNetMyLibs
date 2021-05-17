using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;
using WCFService.IServices;
using WCFService.Service;

namespace WCFService
{
    public class WCFServiceHost
    {
        public ServiceHost host;

        private static WCFServiceHost _instance;
        private static readonly object Obj = new object();

        private WCFServiceHost()
        {
            //私有构造函数
        }

        //多线程单例模式
        public static WCFServiceHost GetInstance()
        {
            if (_instance == null)
            {
                lock (Obj)
                {
                    if (_instance == null)
                    {
                        _instance = new WCFServiceHost();
                    }
                }
            }

            return _instance;
        }

        #region WCF服务开启
        /// <summary>
        /// 开启TestService这项服务
        /// </summary>
        public void HostStart(int listenPort)
        {
            if (host == null || host.State != CommunicationState.Opened)
            {
                System.Threading.ThreadPool.QueueUserWorkItem((arg) =>
                {
                    TimeSpan timeOut = new TimeSpan(0, 1, 0);

                    Uri baseAddress = new Uri(string.Format("net.tcp://{0}:{1}", IPAddress.Any, listenPort));

                    NetTcpBinding binding = new NetTcpBinding();
                    binding.TransactionFlow = false;
                    binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                    binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                    binding.ReceiveTimeout = timeOut;
                    binding.SendTimeout = timeOut;
                    binding.OpenTimeout = timeOut;
                    binding.CloseTimeout = timeOut;
                    binding.ReliableSession.InactivityTimeout = timeOut;
                    binding.Security.Mode = SecurityMode.None;
                    binding.MaxBufferSize = 1073741823;
                    binding.MaxReceivedMessageSize = 1073741823;

                    //开启WCF服务
                    host = new ServiceHost(typeof(TestService), baseAddress);
                    host.AddServiceEndpoint(typeof(ITestService), binding, baseAddress);
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior());
                    host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

                    host.Open();
                });
            }
            else
            {
                MessageBox.Show("端口已打开");
            }
        }
        #endregion

        #region WCF服务关闭
        /// <summary>
        /// 关闭host连接
        /// </summary>
        public void HostClose()
        {
            if (host != null && host.State != CommunicationState.Closed)
            {
                System.Threading.ThreadPool.QueueUserWorkItem((arg) =>
                {
                    host.Close();
                });
            }
        }
        #endregion
    }
}
