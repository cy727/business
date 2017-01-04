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
                MessageBox.Show("���ݿ����Ӵ����������Ա��ϵ");
                strConn = "";
                return;

            }

            MessageBox.Show("���ݿ���������");
            sqlConn.Close();

            dSet.Tables["���ݿ���Ϣ"].Rows[0][0] = textBoxServer.Text;
            dSet.Tables["���ݿ���Ϣ"].Rows[0][1] = textBoxUser.Text;

            if(checkBoxRember.Checked) //��ס����
                dSet.Tables["���ݿ���Ϣ"].Rows[0][2] = textBoxPassword.Text;
            else
                dSet.Tables["���ݿ���Ϣ"].Rows[0][2] = "";

            dSet.Tables["���ݿ���Ϣ"].Rows[0][3] = textBoxDatabase.Text;
            dSet.WriteXml(dFileName);


            this.Close();

        }

        private void formDatabaseSet_Load(object sender, EventArgs e)
        {
            if (intMode == 0)//����
            {
                btnTest.Visible = true;
                btnCreate.Visible = false;
                this.Text = "���ݿ�����";
            }
            else //����
            {
                btnTest.Visible = false;
                btnCreate.Visible = true;
                this.Text = "�������ݿ�";
            }

            sqlComm.Connection = sqlConn;

            if(File.Exists(dFileName)) //�����ļ�
            {
                dSet.ReadXml(dFileName);
            }
            else  //�����ļ�
            {
                dSet.Tables.Add("���ݿ���Ϣ");

                dSet.Tables["���ݿ���Ϣ"].Columns.Add("��������ַ", System.Type.GetType("System.String"));
                dSet.Tables["���ݿ���Ϣ"].Columns.Add("�û���", System.Type.GetType("System.String"));
                dSet.Tables["���ݿ���Ϣ"].Columns.Add("����", System.Type.GetType("System.String"));
                dSet.Tables["���ݿ���Ϣ"].Columns.Add("���ݿ�", System.Type.GetType("System.String"));

                string[]  strDRow ={ "","","",""};
                dSet.Tables["���ݿ���Ϣ"].Rows.Add(strDRow);
            }

            textBoxServer.Text = dSet.Tables["���ݿ���Ϣ"].Rows[0][0].ToString();
            textBoxUser.Text = dSet.Tables["���ݿ���Ϣ"].Rows[0][1].ToString();
            textBoxPassword.Text = dSet.Tables["���ݿ���Ϣ"].Rows[0][2].ToString();
            textBoxDatabase.Text = dSet.Tables["���ݿ���Ϣ"].Rows[0][3].ToString();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("��������в������ݿⴴ��", "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("���ݿⴴ��ʧ�ܣ�" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                
                //MessageBox.Show("���ݿⴴ���ɹ�������sa���µ�¼ϵͳ" , "���ݿ�", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dSet.Tables["���ݿ���Ϣ"].Rows[0][0] = textBoxServer.Text;
                dSet.Tables["���ݿ���Ϣ"].Rows[0][1] = textBoxUser.Text;

                if (checkBoxRember.Checked) //��ס����
                    dSet.Tables["���ݿ���Ϣ"].Rows[0][2] = textBoxPassword.Text;
                else
                    dSet.Tables["���ݿ���Ϣ"].Rows[0][2] = "";

                dSet.Tables["���ݿ���Ϣ"].Rows[0][3] = textBoxDatabase.Text;
                dSet.WriteXml(dFileName);
                

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿⴴ��ʧ�ܣ�" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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