﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Compression;
using System.Xml.Linq;
using System.Xml;

namespace _2lab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Main();
        }
        public List<Question> factors = new List<Question>();
        public List<Factor> targets = new List<Factor>();
        public Node current_node = null;
        private Node contextselected = null;
        private Node tree;
        string project_name = "";
        string project_path = "";
        string project_folder_path = "";
        string perm = ".keks";
        bool unsaved = false;
        /*windows*/
        WinNodes q_and_a;
        Text_exp text_exp;
        enum states { auto, handle };
        states state = states.auto;
        public void Main()
        {
            this.MouseWheel += pictureBox1_MouseWheel;
            tree = new_node(new Point(50, 100));
            saveFileDialog1.Filter = "TreeXP files (*.keks)|*.keks";
            //новыйПроектToolStripMenuItem_Click(new object(), null);
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }
        //Создание нового узла
        private Node new_node(Point pos)
        {
            Node node = new Node();
            pictureBox1.Controls.Add(node);
            node.MouseDown += this.Node_edit;
            node.MouseMove += pictureBox1_MouseMove;
            node.MouseUp += node.Node_MouseUp;
            node.ContextMenuStrip = contextMenu_Node;
            node.Location = pos;
            return node;
        }
        //вызывается при нажатии на кнопку
        public void Place(Node node) { 
            if (node.edited)
                return;
            current_node = node;
            current_node.BackColor = Color.Yellow;
            open_q_and_a();
        }

        //При закрытии окна выбора
        public void Form1_WinNodeClosing(object sender, FormClosingEventArgs e)
        {
            if (current_node == null) return;
            current_node.BackColor = System.Drawing.Color.White;
            Factor f = q_and_a.returner;
            q_and_a = null;
            if (f == null || current_node == null) return;
            if (f != null)
            {
                current_node.f_harness(f);
            }
            if (f.quest)
            {
                Question q = (Question)f;
                if (q.variants.Count == 0) return;
                node_del(current_node);
                if (state == states.handle)
                {
                    double angle = (Math.PI / 2 / (q.variants.Count - 1));
                    for (int i = 0; i < q.variants.Count; i++)
                    {
                        current_node.variants.Add(new_node(new Point(current_node.Location.X + current_node.Width / 2 - 75 / 2 + (int)((200) * Math.Cos(i * angle)), current_node.Location.Y + (int)((200) * Math.Sin(i * angle)))));
                    }
                }
                else if (state == states.auto)
                {
                    tree.check_height();
                    current_node.variants.Add(new_node(new Point(current_node.Location.X + current_node.Width / 2 - 75 / 2 + 200, current_node.Location.Y + 50)));
                    for (int i =1; i< q.variants.Count; i++)
                    {
                        current_node.variants.Add(new_node(new Point(current_node.Location.X + current_node.Width / 2 - 75 / 2 + 200, current_node.variants[i-1].Location.Y + current_node.variants[i-1].height)));
                    }
                }
                RDraw();
            }
        }
        public void node_del(Node n)
        {
            foreach (Node t in n.variants)
            {
                node_del(t);
                pictureBox1.Controls.Remove(t);
            }
            n.variants.Clear();
        }
        public void Node_edit(object node, MouseEventArgs e)
        {
            Node cch = (Node)node;
            if (e.Button == MouseButtons.Left)
            {
                current_node = cch;
                Place(current_node);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                cch.edited = !cch.edited;
                current_node = cch;
            }
            else if (e.Button == MouseButtons.Right)
            {
                contextselected = cch;
                current_node = cch;
            }
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (current_node != null && current_node.edited)
            {

                current_node.Location =new Point(-pictureBox1.Location.X+Cursor.Position.X - Location.X - current_node.Size.Width/2,-pictureBox1.Location.Y + Cursor.Position.Y - Location.Y-30 - current_node.Size.Height / 2);
                RDraw();
            }
        }

        private void RDraw()
        {
            Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 3);
            tree.check_height();
            Drawvar(tree, e.Graphics, pen);
        }
        private void Drawvar(Node node, Graphics k, Pen p)
        {
            Font f = new Font("Calibri", 10, FontStyle.Underline);
            if (state == states.auto) {
            
                if (node.variants.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < node.variants_tips.Count; i++)
                {
                    
                    if (i == 0) {node.variants[0].Location = new Point(node.Location.X + node.Width / 2 - 75 / 2 + 200, node.Location.Y + 50); }
                    else
                        node.variants[i].Location = new Point(node.Location.X + node.Width / 2 - 75 / 2 + 200, node.variants[i - 1].Location.Y + node.variants[i - 1].height);
                    k.DrawLine(p, node.Location + new Size(node.Width / 2, node.Height / 2), new PointF(node.Location.X + node.Width / 2, node.variants[i].Location.Y + node.Height / 2));
                    k.DrawLine(p, new PointF(node.Location.X + node.Width / 2, node.variants[i].Location.Y + node.Height / 2), new PointF(node.variants[i].Location.X + node.variants[i].Width / 2, node.variants[i].Location.Y + node.Height / 2));
                    k.DrawString(node.variants_tips[i].ToString(), f, Brushes.Black, new PointF((node.Location.X + node.variants[i].Location.X) / 2 + node.Width / 2,
                        node.variants[i].Location.Y - 10));
                    Drawvar(node.variants[i], k, p);
                }
            }
            else
            {
                for (int i = 0; i < node.variants.Count; i++)
                {
                    k.DrawLine(p, node.Location + new Size(node.Width / 2, node.Height / 2), node.variants[i].Location + new Size(node.variants[i].Width / 2,
                        node.variants[i].Height / 2));
                    k.DrawString(node.variants_tips[i].ToString(), f, Brushes.Black, new PointF((node.Location.X + node.variants[i].Location.X) / 2 + node.Width / 2,
                        (node.Location.Y + node.variants[i].Location.Y) / 2 - 5));
                    Drawvar(node.variants[i], k, p);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) //Запуск теста
        {
            List<Node> empties = completetion_check(tree);
            if (empties.Count != 0)
            {
                if (DialogResult.OK ==MessageBox.Show("Имеются пустые узлы"))
                {
                    foreach(Node cch in empties)
                    {
                        cch.BackColor = Color.White;
                    }
                }
                return;
            }
            Test start = new Test(tree);
            start.ShowDialog();
        }

        private List<Node> completetion_check(Node node)/*Возвращает список незавершенных узлов*/
        {
            Stack<Node> stack = new Stack<Node>();
            List<Node> empties = new List<Node>();
            stack.Push(node);
            while (stack.Count > 0)
            {
                Node up = stack.Pop();
                foreach (Node i in up.variants)
                {
                    stack.Push(i);
                }
                if (up.child == null)
                {
                    empties.Add(up);
                }
            }
            foreach(Node n in empties)
            {
                n.BackColor = Color.Red;
            }
            return empties;
        }
        public Node Init_node(Node_save node_save, Node tree)
        {
            Factor f = null;
            foreach (Question factor in factors)
            {
                if (factor.Text == node_save.Text)
                {
                    f = factor;
                }
            }
            if (f == null) {
                foreach (Factor target in targets)
                {
                    if (target.Text == node_save.Text)
                    {
                        f = target;
                    }
                } 
            }
            if (f != null)
            {
                tree.f_harness(f);
            }
            
            tree.Location = node_save.Location;
            tree.variants_tips = node_save.variants_tips;
            for (int i =0; i < node_save.variants.Count; i++)
            {
                Node cch = Init_node(node_save.variants[i], new_node(node_save.variants[i].Location));
                tree.variants.Add(cch);
            }
            return tree;
        }

        private void новыйПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project_form prf = new Project_form();
            prf.StartPosition = FormStartPosition.CenterScreen;
            prf.FormClosing += newproject_Closing;
            prf.ShowDialog();
        }
        public void newproject_Closing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.OK == ((Project_form)sender).DialogResult)
            {
                clear_node(tree);
                tree.f_harness();
                RDraw();
                project_name = ((Project_form)(sender)).project_name;
                project_folder_path = ((Project_form)(sender)).default_path;
                сохранитьToolStripMenuItem_Click(this, e);

            }
        }

        private void редакторУзловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            current_node = null;
            open_q_and_a();
        }

        private void open_q_and_a()
        {
            q_and_a = new WinNodes(factors, targets);
            q_and_a.StartPosition = FormStartPosition.CenterScreen;
            q_and_a.FormClosing += this.Form1_WinNodeClosing;
            q_and_a.ShowDialog();
        }

        private void редакторФакторовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_q_and_a();
            q_and_a.addQuestion_Click(sender, e);
        }

        private void редакторЦелейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_q_and_a();
            q_and_a.addAnswer_Click(sender, e);
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            node_del(current_node);
            current_node.child = null;
            current_node.Text = "";
            RDraw();
        }
        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_q_and_a();
            q_and_a.find_and_select(current_node);
            q_and_a.EditQuestion_Click(sender, e);
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(-hScrollBar1.Value * 60, pictureBox1.Location.Y);
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(pictureBox1.Location.X, 40 -vScrollBar1.Value * (pictureBox1.Height / 100));
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            int r = vScrollBar1.Value - e.Delta/50;
            if (r < vScrollBar1.Minimum) r = vScrollBar1.Minimum;
            else if (r > vScrollBar1.Maximum) r = vScrollBar1.Maximum;
            vScrollBar1.Value = r;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (project_folder_path == "")
            {
                новыйПроектToolStripMenuItem_Click(sender, e);
                return;
            }
            string dircache = "//cache-" + DateTime.Now.Ticks;
            Directory.CreateDirectory(project_folder_path + dircache);
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Question>));
            string filename = project_folder_path + dircache;
            System.IO.FileStream file = System.IO.File.Create(filename + "//Questions.xml");
            writer.Serialize(file, factors);
            file.Close();

            writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Factor>));
            file = System.IO.File.Create(filename + "//Factors.xml");
            writer.Serialize(file, targets);
            file.Close();

            writer = new System.Xml.Serialization.XmlSerializer(typeof(Node_save));
            file = System.IO.File.Create(filename + "//Nodes.xml");
            writer.Serialize(file, new Node_save(tree));
            file.Close();

            if (File.Exists(project_folder_path + "//" + "check" + perm))
                File.Delete(project_folder_path + "//" + "check" + perm);
            ZipFile.CreateFromDirectory(project_folder_path + dircache, project_folder_path + "//" + project_name + perm, CompressionLevel.Fastest, false);
            Directory.Delete(project_folder_path + dircache, true);
            unsaved = false;
        }
        private void сохранитьИВыйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            unsaved = true;
            сохранитьToolStripMenuItem_Click(sender, e);
        }

        private void отрытьToolStripMenuItem_Click(object sender, EventArgs e)///Сделать чтобы открывал только нужные файлы
        {
            /*При открытии  удалить холст*/
            openFileDialog1.Filter = "TreeXP files (*.keks)|*.keks";
            openFileDialog1.FileName = "";
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                clear_node(tree);
                tree.f_harness();
                RDraw();
            }
            else return;
            if (openFileDialog1.FileName != "")
            {
                if (!File.Exists(openFileDialog1.FileName))
                {
                    MessageBox.Show("Такого файла не сущетвует");
                    return;
                }
                project_path = openFileDialog1.FileName;
            }
            else return;
            string dircache = "//cache -" + DateTime.Now.Ticks;
            project_folder_path = Path.GetDirectoryName(project_path);
            Directory.CreateDirectory(project_folder_path + dircache);
            ZipFile.ExtractToDirectory(project_path, project_folder_path + dircache);
            string filename = project_folder_path + dircache + "//Questions.xml";
            if (File.Exists(filename))
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(List<Question>));
                System.IO.StreamReader file = new System.IO.StreamReader(filename);
                factors = (List<Question>)reader.Deserialize(file);
                file.Close();
            }
            filename = project_folder_path + dircache + "//Factors.xml";
            if (File.Exists(filename))
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(List<Factor>));
                System.IO.StreamReader file = new System.IO.StreamReader(filename);
                targets = (List<Factor>)reader.Deserialize(file);
                file.Close();

            }
            filename = project_folder_path + dircache + "//Nodes.xml";
            if (File.Exists(filename))
            {
                Node_save tree_save;
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Node_save));
                System.IO.StreamReader file = new System.IO.StreamReader(filename);
                tree_save = (Node_save)reader.Deserialize(file);
                file.Close();
                tree = Init_node(tree_save, tree);
                RDraw();
            }
            Directory.Delete(project_folder_path + dircache, true);


        }

        private void экспортВТекстToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text_exp = new Text_exp(tree);
            text_exp.StartPosition = FormStartPosition.CenterScreen;
            text_exp.ShowDialog();
        }

        private void экспортВКартинкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SvgFileDialog.Filter = "Svg files (*.svg)|*.svg";
            SvgFileDialog.ShowDialog();
            string path = SvgFileDialog.FileName;
            if (path == "")
            {
                return;
            }
            int maxh = find_max_height(tree, 0);
            int maxw = find_max_weight(tree, 0);
            XmlWriterSettings settings = new XmlWriterSettings();
            string svgtext = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine + "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\"" + Environment.NewLine + " \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">" + Environment.NewLine + "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" \t";

            string svgbox = String.Format("viewBox=\"0.00 0.00 {0} {1}\">", maxw, maxh) + Environment.NewLine;
            svgtext += svgbox;
            svgtext += String.Format("<rect width = \"1920.00\" height = \"{0}\" fill = \"white\" />" + Environment.NewLine, maxh) ;
            svgtext += nodeline_to_svg(tree);
            svgtext += node_to_svg(tree);
            svgtext += nodetext_to_svg(tree);
            svgtext += "</svg>";
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                Console.WriteLine(sw.Encoding.ToString());
                sw.WriteLine(svgtext);
            }

        }
        private int find_max_height(Node n, int max)/*Нахождение нижниге значения*/
        {
            int mc = n.Location.Y + n.height;
            if (mc > max)
            {
                max = mc;
            }
            foreach(Node k in n.variants)
            {
                max = find_max_height(k, max);
            }
            return max;
        }
        private int find_max_weight(Node n, int max)
        {
            int mc = n.Location.X + n.Width;
            if (mc > max)
            {
                max = mc;
            }
            foreach (Node k in n.variants)
            {
                max = find_max_weight(k, max);
            }
            return max;
        }
        private string node_to_svg(Node node) //Отрисовка кнопок в svg
        {
            string but = String.Format("<rect x=\"{0}\" y=\"{1}\" rx=\"20\" ry=\"20\" width=\"{2}\" height=\"{3}\" style = \"fill:yellow;stroke:black;stroke-width:5;opacity:0.5\" />", node.Location.X, node.Location.Y,  node.Size.Width, node.Size.Height);
            but += Environment.NewLine;
            foreach (Node n in node.variants)
            {
                but += node_to_svg(n);  
            }
            return but;
        }
        private string nodeline_to_svg(Node node)//Отрисовка линий и подписи в svg
        {
            string lines = "";
            foreach (Node n in node.variants)
            {
                lines += String.Format("<polyline points = \"{0},{1} {2},{3} {4},{5}\" style = \"fill:none;stroke:black;stroke-width:3\" />"+ Environment.NewLine ,node.Location.X + (node.Size.Width/2), node.Location.Y + (node.Size.Height), node.Location.X + (node.Size.Width / 2), n.Location.Y + node.Height / 2, n.Location.X , n.Location.Y + (node.Height/2));
                lines += nodeline_to_svg(n);
            }
            for (int i = 0; i < node.variants_tips.Count; i++)
            {
                lines += String.Format("<text x = \"{0}\" y = \"{1}\" fill = \"black\" font-family = \"Calibri\" font-size = \"12\" text-decoration=\"underline\" >{2}</text>" + Environment.NewLine, (node.Location.X + node.variants[i].Location.X) / 2 + node.Width / 2, node.variants[i].Location.Y - 10, node.variants_tips[i].ToString());
            }
            return lines;
        }
        private string nodetext_to_svg(Node node)
        {
            string lines = "";
            lines += String.Format("<text x = \"{0}\" y = \"{1}\" fill = \"black\" font-family = \"Calibri\" font-size = \"12\" >{2}</text>" + Environment.NewLine, node.Location.X + 10, node.Location.Y + node.Size.Height/2 + 5, node.Text);
            foreach (Node n in node.variants)
            {
                lines += nodetext_to_svg(n);
            }
            return lines;
        }

        private void холстToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear_node(tree);
            tree.f_harness();
            RDraw();
        }
        private void clear_node(Node n)
        {
            foreach (Node t in n.variants)
            {
                clear_node(t);
                pictureBox1.Controls.Remove(t);
            }
            n.variants.Clear();


        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

