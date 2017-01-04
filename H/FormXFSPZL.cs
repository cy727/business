using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXFSPZL : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private ClassGetInformation cGetInformation;

        public int intKFID = 0;
        
        public FormXFSPZL()
        {
            InitializeComponent();
        }

        private void FormXFSPZL_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;


            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            toolStripComboBoxLB.SelectedIndex = 0;
            toolStripButtonGD_Click(null, null);
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                textBoxKFMC.Text = cGetInformation.strKFName;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                initKFView();

            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFBH.Text) == 0)
                {
                    intKFID = 0;
                    textBoxKFMC.Text = "";
                    textBoxKFBH.Text = "";
                    //return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    initKFView();

                }
            }
        }

        private void textBoxKFWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text) == 0)
                {
                    intKFID = 0;
                    textBoxKFMC.Text = "";
                    textBoxKFBH.Text = "";
                    //return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    initKFView();
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            if (intKFID == 0 || toolStripComboBoxLB.SelectedIndex == 0)
            {
                sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 结算付款定义表.将付款数量 AS 下发数量, 商品表.库存数量, 商品表.库存成本价, 商品表.库存金额, 商品表.ID FROM 商品表 CROSS JOIN 结算付款定义表 WHERE (商品表.beactive = 1)";
            }
            else
            {
                sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 商品表.商品规格, 结算付款定义表.将付款数量 AS 下发数量, 商品表.库存数量, 商品表.库存成本价, 商品表.库存金额, 商品表.ID FROM 商品表 INNER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID CROSS JOIN 结算付款定义表 WHERE (商品表.beactive = 1) AND (商品分类表.库房ID = " + intKFID.ToString() + ")";
            }

            
            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");

            dataGridView2.DataSource = dSet.Tables["商品表"];

            dataGridView2.Columns[7].Visible = false;
            dataGridView2.Columns[0].ReadOnly = false;
            dataGridView2.Columns[1].ReadOnly = false;
            dataGridView2.Columns[2].ReadOnly = false;
            dataGridView2.Columns[4].ReadOnly = false;
            dataGridView2.Columns[5].ReadOnly = false;
            dataGridView2.Columns[6].ReadOnly = false;



            sqlConn.Close();

        }

        private void initKFView()
        {
            if(intKFID==0)
            {
                MessageBox.Show("输入下发库房", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sqlConn.Open();

            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, 库存表.库存数量, 库存表.库存金额,  商品表.ID FROM 库存表 INNER JOIN  商品表 ON 库存表.商品ID = 商品表.ID WHERE (库存表.库房ID = " + intKFID.ToString() + ") AND (库存表.BeActive = 1)";
            if (dSet.Tables.Contains("库存表")) dSet.Tables.Remove("库存表");
            sqlDA.Fill(dSet, "库存表");

            dataGridView1.DataSource = dSet.Tables["库存表"];
            dataGridView1.Columns[4].Visible = false;
            sqlConn.Close();
        }

        private void toolStripButtonXF_Click(object sender, EventArgs e)
        {
            int i;
            decimal dTemp1 = 0, dTemp2 = 0;
            decimal fTemp = 0, fTemp1 = 0;
            bool bNo = false;

            if (intKFID == 0)
            {
                MessageBox.Show("输入下发库房", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridView2.SelectedRows.Count < 1)
            {
                MessageBox.Show("选择下发商品和数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //单据明细
                for (i = 0; i < dataGridView2.SelectedRows.Count; i++)
                {
                    if (dataGridView2.SelectedRows[i].IsNewRow)
                        continue;

                    if (dataGridView2.SelectedRows[i].Cells[3].Value.ToString()=="")
                        dataGridView2.SelectedRows[i].Cells[3].Value=0;
                    if (dataGridView2.SelectedRows[i].Cells[4].Value.ToString()=="")
                        dataGridView2.SelectedRows[i].Cells[4].Value=0;
                    if (dataGridView2.SelectedRows[i].Cells[5].Value.ToString()=="")
                        dataGridView2.SelectedRows[i].Cells[5].Value=0;
                    if (dataGridView2.SelectedRows[i].Cells[6].Value.ToString()=="")
                        dataGridView2.SelectedRows[i].Cells[6].Value=0;

                    //总库存
                    dTemp1 = Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[4].Value.ToString()) + Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[3].Value.ToString());
                    dTemp2 = Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[6].Value.ToString()) + Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[3].Value.ToString()) * Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[5].Value.ToString());


                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dTemp1.ToString() + ", 库存金额 = " + dTemp1.ToString() + "*[库存成本价] WHERE (ID = " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //更改分库存
                    fTemp = 0; fTemp1 = 0;
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额 FROM 库存表 WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //库存成本价
                    bNo = false;
                    while (sqldr.Read())
                    {
                        fTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        fTemp1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        bNo = true;
                    }
                    sqldr.Close();

                    dTemp1 = fTemp + Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[3].Value.ToString());
                    dTemp2 = fTemp1 + Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[3].Value.ToString()) * Convert.ToDecimal(dataGridView2.SelectedRows[i].Cells[5].Value.ToString());

                    if(!bNo) //没有库存
                        sqlComm.CommandText = "INSERT INTO 库存表 (库房ID, 商品ID, 库存数量, 库存金额, 库存成本价, BeActive, 库存上限, 库存下限, 合理库存上限, 合理库存下限) VALUES (" + intKFID.ToString() + ", " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ", " + dTemp1.ToString() + ", " + dTemp1.ToString() + "*" + dataGridView2.SelectedRows[i].Cells[5].Value.ToString() + ", " + dataGridView2.SelectedRows[i].Cells[5].Value.ToString() + ", 1, 0, 0, 0, 0)";
                    else //存在库存
                        sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dTemp1.ToString() + ", 库存金额 = " + dTemp1.ToString() + "*[库存成本价] WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridView2.SelectedRows[i].Cells[7].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                }
                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return;
            }
            finally
            {
                sqlConn.Close();
            }


            MessageBox.Show("商品下放到库房完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initKFView();
            toolStripButtonGD_Click(null, null);
        }

        private void toolStripButtonSC_Click(object sender, EventArgs e)
        {
            int i;
            decimal dTemp1 = 0, dTemp2 = 0;
            decimal fTemp = 0, fTemp1 = 0;
            bool bNo = false;

            if (intKFID == 0)
            {
                MessageBox.Show("输入商品库房", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageBox.Show("选择删除商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //单据明细
                for (i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    if (dataGridView1.SelectedRows[i].IsNewRow)
                        continue;

                    if (dataGridView1.SelectedRows[i].Cells[2].Value.ToString() == "")
                        dataGridView1.SelectedRows[i].Cells[2].Value = 0;
                    if (dataGridView1.SelectedRows[i].Cells[3].Value.ToString() == "")
                        dataGridView1.SelectedRows[i].Cells[3].Value = 0;

                    if (dataGridView1.SelectedRows[i].Cells[2].Value.ToString() != "0")
                    {
                        MessageBox.Show("商品" + dataGridView1.SelectedRows[i].Cells[1].Value.ToString() + "库存不为0,无法删除");
                        continue;
                    }

                    sqlComm.CommandText = "DELETE 库存表 WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    /*

                    //分库存
                    sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = 0, 库存金额 = 0, BeActive = 0 WHERE (库房ID = " + intKFID.ToString() + ") AND (商品ID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();


                    //更改总库存
                    fTemp = 0; fTemp1 = 0;
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额 FROM 商品表 WHERE (ID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    //库存成本价
                    while (sqldr.Read())
                    {
                        fTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        fTemp1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                    }
                    sqldr.Close();

                    dTemp1 = fTemp - Convert.ToDecimal(dataGridView1.SelectedRows[i].Cells[2].Value.ToString());
                    dTemp2 = fTemp1 - Convert.ToDecimal(dataGridView1.SelectedRows[i].Cells[3].Value.ToString());

                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dTemp1.ToString() + ", 库存金额 = " + dTemp2.ToString() + " WHERE (ID = " + dataGridView1.SelectedRows[i].Cells[4].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                    */

                }
                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return;
            }
            finally
            {
                sqlConn.Close();
            }


            MessageBox.Show("库房商品删除完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initKFView();
 
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库房商品列表;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库房商品列表;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
        }
    }
}