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

namespace MSMQSender
{
    public partial class FormSender : Form
    {
        private MessageQueue queue;
        private string path = ".\\Private$\\myQueue";

        public FormSender()
        {
            InitializeComponent();
            CreateMessageQueue();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage(this.textBox1.Text);
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

        private void SendMessage(string str)
        {
            System.Messaging.Message message = new System.Messaging.Message();
            message.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            message.Body = str;
            queue.Send(message);
            MessageBox.Show("消息发送成功");
        }

        private void SendMessage(Image image)
        {
            System.Messaging.Message message = new System.Messaging.Message();
            message.Formatter = new BinaryMessageFormatter();
            message.Body = image;
            queue.Send(message);
            MessageBox.Show("图像发送成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "图像文件|*.jpg;*.bmp;*.png";
            open.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (open.ShowDialog() == DialogResult.OK)
            {
                Image image = Bitmap.FromFile(open.FileName);
                SendMessage(image);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(queue.GetAllMessages().Length.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            queue.Purge();
        }
    }
}
