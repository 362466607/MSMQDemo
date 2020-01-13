using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Messaging;

namespace MSMQReceiver
{
    public partial class FormReceiver : Form
    {
        private MessageQueue queue;
        private string path = ".\\Private$\\myQueue";

        public FormReceiver()
        {
            InitializeComponent();
            CreateMessageQueue();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RecvStringMessage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RecvImageMessage();
        }

        private void CreateMessageQueue()
        {
            if (MessageQueue.Exists(path))
            {
                queue = new MessageQueue(path);
            }
            else
            {
                queue = MessageQueue.Create(path);
            }
        }

        public void RecvStringMessage()
        {
            if (queue.GetAllMessages().Length == 0)
            {
                MessageBox.Show("消息队列为空");
                return;
            }
            System.Messaging.Message message = queue.Receive();

            message.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            MessageBox.Show(message.Body.ToString());
        }

        public void RecvImageMessage()
        {
            if (queue.GetAllMessages().Length == 0)
            {
                MessageBox.Show("消息队列为空");
                return;
            }
            System.Messaging.Message message = queue.Receive();

            message.Formatter = new BinaryMessageFormatter();

            Image image = (Image)message.Body;

            Form form = new Form();
            form.Width = 1024;
            form.Height = 768;
            PictureBox pbox = new PictureBox();
            pbox.Width = 1024;
            pbox.Height = 768;
            pbox.SizeMode = PictureBoxSizeMode.Zoom;
            pbox.Image = image;
            form.Controls.Add(pbox);
            form.ShowDialog();
        }
    }
}
