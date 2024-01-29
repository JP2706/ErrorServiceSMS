using Cipher;
using ErrorService.Core;
using ErrorService.Core.Repositories;
using SmsSender;
using System;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace ErrorServiceSMS
{
    public partial class ErrorService : ServiceBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private string _token;
        private string _clientNumber;
        private static int _intervalInMinutes = 8;
        private Timer _timer = new Timer(_intervalInMinutes * 60000);
        private ErrorRepository _errorRepository = new ErrorRepository();
        private GenrateHtmlSms _generateMessage = new GenrateHtmlSms();
        private SMS _sms;
        private StringCipher _stringCipher =
            new StringCipher("E2CE151A-4512-4159-8E23-86C731B53C99");
        private const string NotEncryptedPasswordPrefix = "encrypt:";
        public ErrorService()
        {
            InitializeComponent();
            try
            {
                _token = DecryptSenderSmsToken();
                _clientNumber = ConfigurationManager.AppSettings["ClientNumber"];
                _sms = new SMS(_token);
            }
            catch (Exception ex)
            {

                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWork;
            _timer.Start();
            Logger.Info("Service started...");

        }

        private string DecryptSenderSmsToken()
        {
            var encryptedPassword = ConfigurationManager.AppSettings["Token"];

            if (encryptedPassword.StartsWith(NotEncryptedPasswordPrefix))
            {
                encryptedPassword = _stringCipher
                    .Encrypt(encryptedPassword.Replace(NotEncryptedPasswordPrefix, ""));

                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configFile.AppSettings.Settings["Token"].Value = encryptedPassword;
                configFile.Save();
            }

            return _stringCipher.Decrypt(encryptedPassword);
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                SendError();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
        }    
    

        private void SendError()
        {
            var errors = _errorRepository.GetLastErrors(_intervalInMinutes);

            if (errors == null || !errors.Any())
                return;

            _sms.Send(_clientNumber, _generateMessage.GenrateError(errors, _intervalInMinutes));

            Logger.Info("Error sent.");
        }

        protected override void OnStop()
        {
            Logger.Info("Service stopped...");
        }
    }
}
