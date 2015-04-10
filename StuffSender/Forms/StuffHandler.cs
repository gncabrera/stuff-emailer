using log4net;
using StuffSender.Exceptions;
using StuffSender.Forms.ProgressEvents;
using StuffSender.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StuffSender.Forms
{
    public class StuffHandler
    {
        public ILog logger = LogManager.GetLogger(typeof(StuffHandler));
        public SendActions Actions { get; set; }
        public StuffHandler()
        {
            Files = new List<UploadedFile>();
        }
        public string Title { get; set; }
        public string Notes { get; set; }
        public List<UploadedFile> Files { get; set; }

        public StuffHandler Build()
        {
            if (string.IsNullOrEmpty(Title))
                throw new ValidationException("The stuff must have a Title");

            var nonExisting = Files.Where(f => !File.Exists(f.FilePath));
            if (nonExisting.Any())
            {
                var str = "[" + string.Join("], [", nonExisting.Select(f => f.FilePath)) + "]";
                throw new ValidationException("The folowing files are inexsting: " + str);
            }

            return this;
        }

        public void Send()
        {

            BackgroundWorker bw = new BackgroundWorker();

            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                try
                {
                    if (Actions.ProcessStarted != null)
                        Actions.ProcessStarted(new SendStart
                        {
                            Files = Files
                        });

                    
                    var emailSender = new GmailEmailSender();

                    BackgroundWorker b = o as BackgroundWorker;

                    var percentage = 100 / (Files.Count + 1);

                    var progresses = new List<SendProgress>();
                    for (int i = 0; i < Files.Count; i++)
                    {
                        var todoist = new Todoist();
                        var file = Files.ElementAt(i);
                        var response = todoist.UploadFile(file.FilePath);
                        var p = percentage * (i + 1);
                        var progress = new SendProgress
                        {
                            UploadedFile = file,
                            Progress = p,
                            Response = response,
                            NextFile = i+1 == Files.Count ? null : Files.ElementAt(i+1)
                        };
                        progresses.Add(progress);
                        b.ReportProgress(p, progress);
                    }

                    var urls = string.Join(Environment.NewLine, progresses.Select(p => p.UploadedFile.FileName + ": " + p.Response.file_url));
                    var body = Notes + Environment.NewLine + Environment.NewLine + "Files: " + Environment.NewLine + urls;

                    emailSender.SendMail(Configuration.InboxEmail, Title, body);
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    throw e;
                }
            });

            // what to do when progress changed (update the progress bar for example)
            bw.ProgressChanged += new ProgressChangedEventHandler(
            delegate(object o, ProgressChangedEventArgs args)
            {
                if (Actions.ProgressChange != null)
                    Actions.ProgressChange(args.ProgressPercentage, args.UserState as SendProgress);
            });

            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args)
            {
                if (args.Error != null && Actions.ProcessError != null)
                {
                    Actions.ProcessError(args.Error);
                }
                else if (Actions.ProcessFinished != null)
                    Actions.ProcessFinished(null);
            });

            bw.RunWorkerAsync();
        }

        
    }
}
