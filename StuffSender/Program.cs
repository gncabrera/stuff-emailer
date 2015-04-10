using log4net;
using StuffSender.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StuffSender
{
    static class Program
    {
        static ILog logger = LogManager.GetLogger(typeof(Program));
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main()
        {

            log4net.Config.XmlConfigurator.Configure();

            logger.Info("Initializing Application...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            // Catch all unhandled exceptions
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadExceptionHandler);
            // Catch all unhandled exceptions in all threads.
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

            Application.Run(new SysTrayApp());

            logger.Info("Application initialized!");
        }

        private static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs args)
        {
            try
            {
                HandleException(args.Exception);
            }
            catch { }
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            try
            {
                HandleException(args.ExceptionObject as Exception);
            }
            catch { }
        }

        private static void HandleException(Exception e)
        {
            var title = "An error has ocurred!";
            var message = "Please, see the logs for more information.";
            if (e is IUnloggableException)
            {
                var exc = e as IUnloggableException;
                title = exc.Title;
                message = e.Message;
            }
            else
            {
                logger.Error(e);
            }
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
