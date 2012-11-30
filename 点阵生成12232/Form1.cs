using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 点阵生成12232
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            initFrame();
        }

        int pX = 0; //控件的XY坐标
        int pY = 0;
        Control pOld = null;    //记录控件的老状态
        GroupBox groupBoxControl = new GroupBox();
        int[,] allData = new int[16,16];

        /// <summary>
        /// 初始化
        /// </summary>
        private void initFrame()
        {
            pX = 15;
            pY = 18;
            pOld = null;
            //this.Size = new System.Drawing.Size(350,500);
            createButton();
        }

        /// <summary>
        /// 创建16*16按钮
        /// </summary>
        private void createButton()
        {
            groupBoxControl.AutoSize = true;
            groupBoxControl.Text = "操作框";
            groupBoxControl.Location = new Point(12, 10);
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    allData[i, j] = 0;
                    Button btnCreate = new Button();
                    btnCreate.FlatStyle = FlatStyle.Flat;
                    btnCreate.Text = "0";
                    btnCreate.Name = i.ToString() + j.ToString(); ;
                    btnCreate.BackColor = Color.White;
                    btnCreate.Width = 20;
                    btnCreate.Height = 20;
                    btnCreate.Location = new Point(pX, pY);
                    btnCreate.MouseMove += new MouseEventHandler(mouseMove);
                    btnCreate.MouseEnter += new EventHandler(mouseEnter);
                    btnCreate.MouseLeave += new EventHandler(mouseLeave);
                    pX += 20;
                    btnCreate.Parent = groupBoxControl;
                }
                pX = 15;
                pY += 20;
            }
            Controls.Add(groupBoxControl);
        }
        /// <summary>
        /// 切换按钮状态
        /// </summary>
        private void changeBtnState(Control pControl)
        {
            if (int.Parse(pControl.Text) == 0)
            {
                pControl.Text = "1";
                pControl.BackColor = Color.Black;
                pControl.ForeColor = Color.Red;
            }
            else
            {
                pControl.Text = "0";
                pControl.BackColor = Color.White;
                pControl.ForeColor = SystemColors.ControlText;

            }
        }
        /// <summary>
        /// 清除现有状态
        /// </summary>
        private void clearState()
        {
            foreach (Control c in groupBoxControl.Controls)
            {
                if (c.Name != "btnCalc" && c.Name != "btnClear")
                {
                    c.Text = "0";
                    c.BackColor = Color.White;
                    c.ForeColor = SystemColors.ControlText;
                    allData = null;
                    allData = new int[16,16];
                    tbxResult.Text = "请重新选择计算";
                }
            }
        }
        //计算4个数之间的数
        private int getBetweenFour(int i)
        {
            if(i>=0&&i<4)
                return 0;
            else if(i>=4&&i<8)
                return 1;
            else if(i>=8&i<12)
                return 2;
            else
                return 3;
        }
        //计算最终结果
        private string calcResult()
        {
            string strResult = "" ;
            int[,] sum = new int[4,16];
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    foreach (Control c in groupBoxControl.Controls)
                    {
                        if (c.Name != "btnCalc" && c.Name != "btnClear")
                        {
                            if (c.Name.Equals(i.ToString() + j.ToString()))
                            {
                                allData[i, j] = int.Parse(c.Text);
                               // Console.Write(allData[i, j].ToString());
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {  
                    int x=getBetweenFour(i);
                    switch (i%4)
                    {
                        case 0:
                            sum[x,j] += allData[i,j];
                            break;
                        case 1:
                            sum[x,j] += allData[i, j] * 2;
                            break;
                        case 2:
                            sum[x,j] += allData[i, j] * 4;
                            break;
                        case 3:
                            sum[x,j] += allData[i, j] * 8;
                            break;
                    }
                }                
            }
            string[,] tem = new string[4,16];
            string[] a = new string[16];
            string[] b = new string[16];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    tem[i,j] = sum[i, j].ToString("X");
                    Console.Write(tem[i, j]);
                }
                Console.WriteLine();
            }
            for (int i = 3;i >= 0; i--)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (i > 1)
                        a[j] += tem[i, j];
                    else
                        b[j] += tem[i, j];
                }
            }
            for (int i = 0; i < 16; i++)
            {
                strResult += "0x" + b[i] + ",";
            }
            strResult += "\r\n";
            for(int i=0;i<16;i++)
            {
                strResult += "0x"+a[i]+",";
            }         
            return strResult;             
                    
        }

        void mouseEnter(object sender, EventArgs e)
        {
            this.Capture = true;
        }
        void mouseLeave(object sender, EventArgs e)
        {
            this.Capture = false;
        }        
        void mouseMove(object sender, MouseEventArgs e)
        {
            Control pControl = sender as Control;            
            if (pOld!=pControl&&e.Button==MouseButtons.Left)
            {
                pOld = pControl;
                changeBtnState(pControl);
            }
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            tbxResult.Text=calcResult();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearState();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxResult.Text))
            {
                Clipboard.SetText(tbxResult.Text);
                MessageBox.Show("数据复制成功");
            }
            else
            {
                MessageBox.Show("文本框数据为空");
            }
        }
    }
}
