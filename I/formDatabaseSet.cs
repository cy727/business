using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;


namespace business
{
    public partial class formDatabaseSet : Form
    {
        public string strConn="";
        private string dFileName="";
        public int intMode = 0;

        private System.Data.DataSet dSet = new DataSet();

        public formDatabaseSet()
        {
            InitializeComponent();
            dFileName = Directory.GetCurrentDirectory() + "\\appcon.xml";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            strConn = "";
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            
            strConn = "workstation id=CY;packet size=4096;user id=" + textBoxUser.Text.Trim().ToLower() + ";password=" + textBoxPassword.Text.Trim().ToLower() + ";data source=\"" + textBoxServer.Text.Trim() + "\";;initial catalog="+textBoxDatabase.Text.Trim().ToLower();

            sqlConn.ConnectionString = strConn;
            try
            {
                sqlConn.Open();
            }
            catch (System.Data.SqlClient.SqlException err)
            {
                MessageBox.Show("数据库连接错误，请与管理员联系");
                strConn = "";
                return;

            }

            MessageBox.Show("数据库连接正常");
            sqlConn.Close();

            dSet.Tables["数据库信息"].Rows[0][0] = textBoxServer.Text;
            dSet.Tables["数据库信息"].Rows[0][1] = textBoxUser.Text;

            if(checkBoxRember.Checked) //记住密码
                dSet.Tables["数据库信息"].Rows[0][2] = textBoxPassword.Text;
            else
                dSet.Tables["数据库信息"].Rows[0][2] = "";

            dSet.Tables["数据库信息"].Rows[0][3] = textBoxDatabase.Text;
            dSet.WriteXml(dFileName);


            this.Close();

        }

        private void formDatabaseSet_Load(object sender, EventArgs e)
        {
            if (intMode == 0)//测试
            {
                btnTest.Visible = true;
                btnCreate.Visible = false;
                this.Text = "数据库设置";
            }
            else //创建
            {
                btnTest.Visible = false;
                btnCreate.Visible = true;
                this.Text = "创建数据库";
            }

            sqlComm.Connection = sqlConn;

            if(File.Exists(dFileName)) //存在文件
            {
                dSet.ReadXml(dFileName);
            }
            else  //建立文件
            {
                dSet.Tables.Add("数据库信息");

                dSet.Tables["数据库信息"].Columns.Add("服务器地址", System.Type.GetType("System.String"));
                dSet.Tables["数据库信息"].Columns.Add("用户名", System.Type.GetType("System.String"));
                dSet.Tables["数据库信息"].Columns.Add("密码", System.Type.GetType("System.String"));
                dSet.Tables["数据库信息"].Columns.Add("数据库", System.Type.GetType("System.String"));

                string[]  strDRow ={ "","","",""};
                dSet.Tables["数据库信息"].Rows.Add(strDRow);
            }

            textBoxServer.Text = dSet.Tables["数据库信息"].Rows[0][0].ToString();
            textBoxUser.Text = dSet.Tables["数据库信息"].Rows[0][1].ToString();
            textBoxPassword.Text = dSet.Tables["数据库信息"].Rows[0][2].ToString();
            textBoxDatabase.Text = dSet.Tables["数据库信息"].Rows[0][3].ToString();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本版软件尚不能数据库创建", "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;

            /*
            strConn = "packet size=4096;user id=" + textBoxUser.Text.Trim().ToLower() + ";password=" + textBoxPassword.Text.Trim().ToLower() + ";data source=\"" + textBoxServer.Text.Trim() + "\";initial catalog=;Integrated Security=True";

            try
            {
                sqlConn.ConnectionString = strConn;
                sqlConn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库创建失败：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strConn = "";
                return;
            }

            try

            {
                sqlComm.CommandText = "create database " + textBoxDatabase.Text.Trim().ToLower();
                sqlComm.ExecuteNonQuery();

                sqlConn.Close();

                strConn = "packet size=4096;user id=" + textBoxUser.Text.Trim().ToLower() + ";password=" + textBoxPassword.Text.Trim().ToLower() + ";data source=\"" + textBoxServer.Text.Trim() + "\";Database=" + textBoxDatabase.Text.Trim().ToLower() + ";Integrated Security=SSPI";
                sqlConn.ConnectionString = strConn;
                sqlConn.Open();

                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                
                //MessageBox.Show("数据库创建成功，请用sa重新登录系统" , "数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dSet.Tables["数据库信息"].Rows[0][0] = textBoxServer.Text;
                dSet.Tables["数据库信息"].Rows[0][1] = textBoxUser.Text;

                if (checkBoxRember.Checked) //记住密码
                    dSet.Tables["数据库信息"].Rows[0][2] = textBoxPassword.Text;
                else
                    dSet.Tables["数据库信息"].Rows[0][2] = "";

                dSet.Tables["数据库信息"].Rows[0][3] = textBoxDatabase.Text;
                dSet.WriteXml(dFileName);
                

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库创建失败：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strConn = "";
            }
            finally
            {
                sqlConn.Close();
                this.Dispose();
            }
         */

        }
    }
}