using StuffSender.Forms.ProgressEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StuffSender.Forms
{
    public partial class SenderForm : Form
    {
        public BindingList<UploadedFile> Files { get; set; }
        public BindingSource BindingSource { get; set; }

        public SendActions SendActions { get; set; }

        public SenderForm()
        {
            InitializeComponent();

            Files = new BindingList<UploadedFile>();

            BindingSource = new BindingSource();
            BindingSource.DataSource = Files;

            lstFilenames.DataSource = BindingSource;
            lstFilenames.DisplayMember = "FileName";
            lstFilenames.ValueMember = "FilePath";

            Activated += SenderForm_Activated;

            this.KeyDown += SenderForm_KeyDown;
            txtTitle.KeyDown += SenderForm_KeyDown;
            txtNotes.KeyDown += SenderForm_KeyDown;
            btnUpload.KeyDown += SenderForm_KeyDown;
            btnDelete.KeyDown += SenderForm_KeyDown;
            lstFilenames.KeyDown += SenderForm_KeyDown;
        }

        void SenderForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.Enter))
            {
                btnSend.PerformClick();
            }
        }

        void SenderForm_Activated(object sender, EventArgs e)
        {
            txtTitle.Focus();
        }

        private void selectFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var dialog = sender as OpenFileDialog;
            AddFiles(dialog.FileNames);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            DialogResult result = selectFileDialog.ShowDialog();
        }

        private void AddFiles(string [] filePaths)
        {
            var files = filePaths.Select(f => new UploadedFile
            {
                FilePath = f
            });

            foreach (var file in files)
            {
                Files.Add(file);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selected = lstFilenames.SelectedItem as UploadedFile;
            Files.Remove(selected);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var handler = new StuffHandler
            {
                Title = txtTitle.Text,
                Notes = txtNotes.Text,
                Files = Files.ToList(),
                Actions = SendActions
            };
            handler.Build().Send();
            Close();
        }

    }
}
