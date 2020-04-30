using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;


namespace Maze
{
    public partial class Form1 : Form
    {
        //
        public int sizelong;
        public int colCount;
        public int rowCount;
        // 所在地坐标
        public int localX;
        public int localY;
        // 目的地坐标
        public int remoteX;
        public int remoteY;


        int dataGridView1MaxWidth;
        int dataGridView1MaxLength;

        // 存放生成的迷宫数据
        const int size = 1000;
        bool[][] Maze = new bool[size][];

        // 找到迷宫的标志位
        public bool findFlag;
        public Form1()
        {
            InitializeComponent();

            // 初始化迷宫数据
            for (int i = 0; i < size; i++)
            {
                Maze[i] = new bool[size];
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
                return;
            // 清空数据
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Clear();
            // 得到矩阵的大小
            sizelong = int.Parse(textBox1.Text);
            colCount = sizelong;
            rowCount = sizelong;
            if (true)
            {
                dataGridView1MaxWidth = this.dataGridView1.Width;
                dataGridView1MaxLength = this.dataGridView1.Height;
                int rangeWidth = (dataGridView1MaxWidth / colCount) < (dataGridView1MaxLength / rowCount) ?
                    (dataGridView1MaxWidth / colCount) : (dataGridView1MaxLength / rowCount);

                this.dataGridView1.ColumnHeadersVisible = false;
                this.dataGridView1.RowHeadersVisible = false;
                //列宽:
                int colWidth = rangeWidth;
                //this.dataGridView1.Width = colWidth * colCount+3 ;
                //行高:
                int rowHeight = rangeWidth;
                //this.dataGridView1.Height = rangeWidth * rowCount + 50;
                for (int j = 0; j < colCount; j++)
                {
                    DataGridViewTextBoxColumn dc = new DataGridViewTextBoxColumn();
                    dc.Width = colWidth;
                    dataGridView1.Columns.Add(dc);
                }
                for (int i = 0; i < rowCount; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Height = rowHeight;
                    dataGridView1.Rows.Add(row);
                }
                //禁止用户改变DataGridView1所有行的行高 
                dataGridView1.AllowUserToResizeRows = false;
                //禁止用户改变DataGridView1所有列的列宽 
                dataGridView1.AllowUserToResizeColumns = false;

                // 创建迷宫
                CreateMaze();
                // 及时更新
                dataGridView1.Refresh();
            }
        }

        private void CreateMaze()
        {
            // 清空迷宫数据
            for (int i = 0; i < sizelong; i++)
            {
                for (int j = 0; j < sizelong; j++)
                    Maze[i][j] = false;
            }
            // true表示为路  false表示为墙
            //最外围设置为路，可以有效的保护里面一层墙体，并防止挖出界
            for (int i = 0; i< sizelong; i++) {
		        Maze[i][0] = true;
		        Maze[0][i] = true;
		        Maze[sizelong - 1][i] = true;
		        Maze[i][sizelong - 1] = true;
	        }

            Random rd = new Random();
            
            //墙队列，包括X , Y
            ArrayList X = new ArrayList();
            ArrayList Y = new ArrayList();

            //任取初始值
            X.Add(2);
	        Y.Add(2);

	            //当墙队列为空时结束循环
	        while (X.Count != 0) {
		            //在墙队列中随机取一点
		        int r = rd.Next() % X.Count;
                int x = (int)X[r];
                int y = (int)Y[r];

                //判读上下左右四个方向是否为路
                int count = 0;
		        for (int i = x - 1; i<x + 2; i++) {
			        for (int j = y - 1; j<y + 2; j++) {
				        if (Math.Abs(x - i) + Math.Abs(y - j) == 1 && Maze[i][j]) {
					        count++;
				        }
			        }
		        }
		        if (count <= 1) {
			        Maze[x][y] = true;
			        //在墙队列中插入新的墙
			        for (int i = x - 1; i<x + 2; i++) {
				        for (int j = y - 1; j<y + 2; j++) {
					        if (Math.Abs(x - i) + Math.Abs(y - j) == 1 && !Maze[i][j]) {
						        X.Add(i);
						        Y.Add(j);
					        }
				        }
			        }
		        }

		        //删除当前墙
		        X.RemoveAt( r);
		        Y.RemoveAt( r);
	        }

	        //设置迷宫进出口
	        Maze[2][1] = true;
	        for (int i = sizelong - 3; i >= 0; i--) {
		        if (Maze[i][sizelong - 3] == true) {
			        Maze[i][sizelong - 2] = true;
			        break;
		        }
	        }

            //改变迷宫的dataGridView颜色
            for (int i = 0; i< colCount; i++){
		        for (int j = 0; j< rowCount; j++)
                {
                    if (Maze[i][j])
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                    else
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Black;
                }
	        }

            // 设置默认所在地
            localX = 1;
            localY = 2;
            dataGridView1.Rows[localY].Cells[localX].Style.BackColor = Color.Yellow;
            this.dataGridView1.Rows[localY].Cells[localX].Selected = true;

            // 设置默认目的地
            remoteX = sizelong-2;
            remoteY = sizelong-3;
            dataGridView1.Rows[remoteY].Cells[remoteX].Style.BackColor = Color.Red;

        }

        private bool CheckIsLegal(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < sizelong && y < sizelong)
                return true;
            else
                return false;
        }
        private void DFS()
        {
            findFlag = false;
            const int size = 100;
            int[][] isArrive = new int[size][];
            for (int i = 0; i < size; i++)
            {
                isArrive[i] = new int[size];
                for (int j = 0; j < size; j++)
                    isArrive[i][j] = 0;
            }
            
            //墙队列，包括X , Y
            ArrayList X = new ArrayList();
            ArrayList Y = new ArrayList();

            //任取初始值
            X.Add(localX);
            Y.Add(localY);
            isArrive[localX][localY] = 1;

            int [] aroundA = new int[4] {1, 0, 0 ,-1 };
            int [] aroundB = new int[4] {0, 1, -1, 0 };

            while (X.Count != 0 && !findFlag)
            {
                int x = (int)X[X.Count-1];
                int y = (int)Y[Y.Count-1];
                int tmpx;
                int tmpy;
                bool nowTimeSucc = false;
                for (int i = 0; i < 4; i++)
                {
                    tmpx = x + aroundA[i];
                    tmpy = y + aroundB[i];
                    if (!CheckIsLegal(tmpx, tmpy))
                        continue;

                    if (tmpx == remoteX && tmpy == remoteY)
                    {
                        findFlag = true;
                        break;
                    }
                    if (isArrive[tmpx][tmpy] == 1)
                        continue;
                    if (this.dataGridView1.Rows[tmpy].Cells[tmpx].Style.BackColor == Color.White)
                    {
                        X.Add(tmpx);
                        Y.Add(tmpy);
                        isArrive[tmpx][tmpy] = 1;
                        nowTimeSucc = true;
                        this.dataGridView1.Rows[tmpy].Cells[tmpx].Style.BackColor = Color.Green;
                        if( checkBox1.Checked == true )
                        {
                            // 及时更新
                            dataGridView1.Refresh();
                        }
                        break ;
                    }
                }
                // 本轮是否添加成功
                if(nowTimeSucc == false && !findFlag)
                {
                    //isArrive[x][y] = 0;
                    this.dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.White;
                    X.RemoveAt(X.Count - 1);
                    Y.RemoveAt(Y.Count - 1);
                    if (checkBox1.Checked == true)
                    {
                        // 及时更新
                        dataGridView1.Refresh();
                    }
                }
                else
                    nowTimeSucc = false;
             }

            int cnt = X.Count;
            for(int i=1; i<cnt; i++ )
            {
                int x = (int)X[i];
                int y = (int)Y[i];
                this.dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.Green;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DFS();
            // 及时更新
            dataGridView1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 恢复上一个状态
            this.dataGridView1.Rows[localY].Cells[localX].Style.BackColor = Maze[localY][localX] ? Color.White : Color.Black;
            // 更新当前选择的单元格
            localX = this.dataGridView1.CurrentCell.ColumnIndex;
            localY = this.dataGridView1.CurrentCell.RowIndex;

            this.dataGridView1.Rows[localY].Cells[localX].Selected = false;
            this.dataGridView1.Rows[localY].Cells[localX].Style.BackColor = Color.Yellow;
            dataGridView1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 恢复上一个状态
            this.dataGridView1.Rows[remoteY].Cells[remoteX].Style.BackColor = Maze[remoteY][remoteX]? Color.White : Color.Black;
            // 更新当前选择的单元格
            remoteX = this.dataGridView1.CurrentCell.ColumnIndex;
            remoteY = this.dataGridView1.CurrentCell.RowIndex;

            this.dataGridView1.Rows[remoteY].Cells[remoteX].Selected = false;
            this.dataGridView1.Rows[remoteY].Cells[remoteX].Style.BackColor = Color.Red;
            dataGridView1.Refresh();
        }
    }
}
