using System;
using System.Configuration;
using Prism.Commands;
using Utils;
using WCFService;

namespace wpfSimulation.ViewModels
{
    public class ModifyWcfListenPortViewModel : BaseViewModels
    {
        //private Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public ModifyWcfListenPortViewModel()
        {
            BtnOkCommand = new DelegateCommand(BtnOkCommandDo);
            BtnCancelCommand = new DelegateCommand(BtnCancelCommandDo);

            //读取配置文件的端口号
            ListernPort = ConfigurationManager.AppSettings["ListenPort"] == string.Empty
                ? 8000
                : Convert.ToInt32(ConfigurationManager.AppSettings["ListenPort"]);
        }

        #region DIalogResult数据绑定

        /// <summary>
        /// 前提：DialogResult不是依赖属性，使用DialogCloser类注册依赖属性
        /// </summary>
        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                OnPropertyChanged("DialogResult");
            }
        }

        #endregion

        #region UI绑定属性

        /// <summary>
        /// TextBox端口号
        /// </summary>
        private int _listernPort = 0;

        public int ListernPort
        {
            get => _listernPort;
            set
            {
                _listernPort = value;
                OnPropertyChanged("ListernPort");
            }
        }

        #endregion

        #region Button Command
        public DelegateCommand BtnOkCommand { get; private set; }
        public DelegateCommand BtnCancelCommand { get; private set; }

        /// <summary>
        /// Ok Button
        /// </summary>
        private void BtnOkCommandDo()
        {
            //将端口号记录配置文件
            ConfigurationHelper.AddUpdateAppSettings("ListenPort", ListernPort.ToString());

            //开启WCF服务
            WCFServiceHost.GetInstance().HostStart(ListernPort);
            this.DialogResult = true;
        }

        /// <summary>
        /// Cancel Button
        /// </summary>
        private void BtnCancelCommandDo()
        {
            DialogResult = false;
        }
        #endregion
    }
}