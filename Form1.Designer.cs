namespace ats
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.jd_kw_openAPI = new AxKHOpenAPILib.AxKHOpenAPI();
            this.jd_mns_top = new System.Windows.Forms.MenuStrip();
            this.jd_mns_login = new System.Windows.Forms.ToolStripMenuItem();
            this.jd_mns_logout = new System.Windows.Forms.ToolStripMenuItem();
            this.jd_grp_user_info = new System.Windows.Forms.GroupBox();
            this.jd_cmb_acc_no = new System.Windows.Forms.ComboBox();
            this.jd_lbl_acc_no = new System.Windows.Forms.Label();
            this.jd_lbl_user_id = new System.Windows.Forms.Label();
            this.jd_txt_user_id = new System.Windows.Forms.TextBox();
            this.jd_grp_trd = new System.Windows.Forms.GroupBox();
            this.jd_btn_remove = new System.Windows.Forms.Button();
            this.jd_btn_update = new System.Windows.Forms.Button();
            this.jd_btn_insert = new System.Windows.Forms.Button();
            this.jd_btn_search = new System.Windows.Forms.Button();
            this.jd_dgv_main = new System.Windows.Forms.DataGridView();
            this.jd_grp_auto = new System.Windows.Forms.GroupBox();
            this.jd_btn_stop = new System.Windows.Forms.Button();
            this.jd_btn_start = new System.Windows.Forms.Button();
            this.jd_grp_msg = new System.Windows.Forms.GroupBox();
            this.jd_txt_msg = new System.Windows.Forms.TextBox();
            this.jd_grp_err = new System.Windows.Forms.GroupBox();
            this.jd_txt_err = new System.Windows.Forms.TextBox();
            this.jd_ssr_bottom = new System.Windows.Forms.StatusStrip();
            this.jd_ssr_label1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.seq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jongmok_cd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jongmok_nm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buy_amt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buy_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.target_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cut_loss_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buy_trd_yn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.sell_trd_yn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.jd_kw_openAPI)).BeginInit();
            this.jd_mns_top.SuspendLayout();
            this.jd_grp_user_info.SuspendLayout();
            this.jd_grp_trd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jd_dgv_main)).BeginInit();
            this.jd_grp_auto.SuspendLayout();
            this.jd_grp_msg.SuspendLayout();
            this.jd_grp_err.SuspendLayout();
            this.jd_ssr_bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // jd_kw_openAPI
            // 
            this.jd_kw_openAPI.Enabled = true;
            this.jd_kw_openAPI.Location = new System.Drawing.Point(1224, 41);
            this.jd_kw_openAPI.Name = "jd_kw_openAPI";
            this.jd_kw_openAPI.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("jd_kw_openAPI.OcxState")));
            this.jd_kw_openAPI.Size = new System.Drawing.Size(74, 45);
            this.jd_kw_openAPI.TabIndex = 0;
            this.jd_kw_openAPI.OnReceiveTrData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEventHandler(this.axKHOpenAPI1_OnReceiveTrData);
            // 
            // jd_mns_top
            // 
            this.jd_mns_top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jd_mns_login,
            this.jd_mns_logout});
            this.jd_mns_top.Location = new System.Drawing.Point(0, 0);
            this.jd_mns_top.Name = "jd_mns_top";
            this.jd_mns_top.Size = new System.Drawing.Size(1098, 24);
            this.jd_mns_top.TabIndex = 1;
            this.jd_mns_top.Text = "menuStrip1";
            this.jd_mns_top.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // jd_mns_login
            // 
            this.jd_mns_login.Name = "jd_mns_login";
            this.jd_mns_login.Size = new System.Drawing.Size(55, 20);
            this.jd_mns_login.Text = "로그인";
            this.jd_mns_login.Click += new System.EventHandler(this.로그인ToolStripMenuItem_Click);
            // 
            // jd_mns_logout
            // 
            this.jd_mns_logout.Name = "jd_mns_logout";
            this.jd_mns_logout.Size = new System.Drawing.Size(67, 20);
            this.jd_mns_logout.Text = "로그아웃";
            this.jd_mns_logout.Click += new System.EventHandler(this.로그아웃ToolStripMenuItem_Click);
            // 
            // jd_grp_user_info
            // 
            this.jd_grp_user_info.Controls.Add(this.jd_cmb_acc_no);
            this.jd_grp_user_info.Controls.Add(this.jd_lbl_acc_no);
            this.jd_grp_user_info.Controls.Add(this.jd_lbl_user_id);
            this.jd_grp_user_info.Controls.Add(this.jd_txt_user_id);
            this.jd_grp_user_info.Location = new System.Drawing.Point(12, 27);
            this.jd_grp_user_info.Name = "jd_grp_user_info";
            this.jd_grp_user_info.Size = new System.Drawing.Size(850, 44);
            this.jd_grp_user_info.TabIndex = 2;
            this.jd_grp_user_info.TabStop = false;
            this.jd_grp_user_info.Text = "접속정보";
            this.jd_grp_user_info.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // jd_cmb_acc_no
            // 
            this.jd_cmb_acc_no.FormattingEnabled = true;
            this.jd_cmb_acc_no.Location = new System.Drawing.Point(318, 14);
            this.jd_cmb_acc_no.Name = "jd_cmb_acc_no";
            this.jd_cmb_acc_no.Size = new System.Drawing.Size(210, 20);
            this.jd_cmb_acc_no.TabIndex = 3;
            this.jd_cmb_acc_no.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // jd_lbl_acc_no
            // 
            this.jd_lbl_acc_no.AutoSize = true;
            this.jd_lbl_acc_no.Location = new System.Drawing.Point(231, 17);
            this.jd_lbl_acc_no.Name = "jd_lbl_acc_no";
            this.jd_lbl_acc_no.Size = new System.Drawing.Size(81, 12);
            this.jd_lbl_acc_no.TabIndex = 4;
            this.jd_lbl_acc_no.Text = "증권계좌번호:";
            this.jd_lbl_acc_no.Click += new System.EventHandler(this.label2_Click);
            // 
            // jd_lbl_user_id
            // 
            this.jd_lbl_user_id.AutoSize = true;
            this.jd_lbl_user_id.Location = new System.Drawing.Point(6, 20);
            this.jd_lbl_user_id.Name = "jd_lbl_user_id";
            this.jd_lbl_user_id.Size = new System.Drawing.Size(45, 12);
            this.jd_lbl_user_id.TabIndex = 3;
            this.jd_lbl_user_id.Text = "아이디:";
            this.jd_lbl_user_id.Click += new System.EventHandler(this.label1_Click);
            // 
            // jd_txt_user_id
            // 
            this.jd_txt_user_id.Location = new System.Drawing.Point(57, 14);
            this.jd_txt_user_id.Name = "jd_txt_user_id";
            this.jd_txt_user_id.Size = new System.Drawing.Size(168, 21);
            this.jd_txt_user_id.TabIndex = 3;
            this.jd_txt_user_id.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // jd_grp_trd
            // 
            this.jd_grp_trd.Controls.Add(this.jd_btn_remove);
            this.jd_grp_trd.Controls.Add(this.jd_btn_update);
            this.jd_grp_trd.Controls.Add(this.jd_btn_insert);
            this.jd_grp_trd.Controls.Add(this.jd_btn_search);
            this.jd_grp_trd.Controls.Add(this.jd_dgv_main);
            this.jd_grp_trd.Location = new System.Drawing.Point(12, 77);
            this.jd_grp_trd.Name = "jd_grp_trd";
            this.jd_grp_trd.Size = new System.Drawing.Size(1075, 249);
            this.jd_grp_trd.TabIndex = 3;
            this.jd_grp_trd.TabStop = false;
            this.jd_grp_trd.Text = "거래 종목 설정";
            this.jd_grp_trd.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // jd_btn_remove
            // 
            this.jd_btn_remove.Location = new System.Drawing.Point(461, 20);
            this.jd_btn_remove.Name = "jd_btn_remove";
            this.jd_btn_remove.Size = new System.Drawing.Size(118, 23);
            this.jd_btn_remove.TabIndex = 4;
            this.jd_btn_remove.Text = "삭제";
            this.jd_btn_remove.UseVisualStyleBackColor = true;
            this.jd_btn_remove.Click += new System.EventHandler(this.button4_Click);
            // 
            // jd_btn_update
            // 
            this.jd_btn_update.Location = new System.Drawing.Point(325, 20);
            this.jd_btn_update.Name = "jd_btn_update";
            this.jd_btn_update.Size = new System.Drawing.Size(130, 23);
            this.jd_btn_update.TabIndex = 3;
            this.jd_btn_update.Text = "수정";
            this.jd_btn_update.UseVisualStyleBackColor = true;
            this.jd_btn_update.Click += new System.EventHandler(this.button3_Click);
            // 
            // jd_btn_insert
            // 
            this.jd_btn_insert.Location = new System.Drawing.Point(179, 20);
            this.jd_btn_insert.Name = "jd_btn_insert";
            this.jd_btn_insert.Size = new System.Drawing.Size(140, 23);
            this.jd_btn_insert.TabIndex = 2;
            this.jd_btn_insert.Text = "삽입";
            this.jd_btn_insert.UseVisualStyleBackColor = true;
            this.jd_btn_insert.Click += new System.EventHandler(this.button2_Click);
            // 
            // jd_btn_search
            // 
            this.jd_btn_search.Location = new System.Drawing.Point(20, 20);
            this.jd_btn_search.Name = "jd_btn_search";
            this.jd_btn_search.Size = new System.Drawing.Size(153, 23);
            this.jd_btn_search.TabIndex = 1;
            this.jd_btn_search.Text = "조회";
            this.jd_btn_search.UseVisualStyleBackColor = true;
            this.jd_btn_search.Click += new System.EventHandler(this.button1_Click);
            // 
            // jd_dgv_main
            // 
            this.jd_dgv_main.BackgroundColor = System.Drawing.SystemColors.Control;
            this.jd_dgv_main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.jd_dgv_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jd_dgv_main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.seq,
            this.jongmok_cd,
            this.jongmok_nm,
            this.priority,
            this.buy_amt,
            this.buy_price,
            this.target_price,
            this.cut_loss_price,
            this.buy_trd_yn,
            this.sell_trd_yn,
            this.check});
            this.jd_dgv_main.Location = new System.Drawing.Point(20, 49);
            this.jd_dgv_main.Name = "jd_dgv_main";
            this.jd_dgv_main.RowHeadersVisible = false;
            this.jd_dgv_main.RowTemplate.Height = 23;
            this.jd_dgv_main.Size = new System.Drawing.Size(1040, 189);
            this.jd_dgv_main.TabIndex = 0;
            this.jd_dgv_main.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // jd_grp_auto
            // 
            this.jd_grp_auto.Controls.Add(this.jd_btn_stop);
            this.jd_grp_auto.Controls.Add(this.jd_btn_start);
            this.jd_grp_auto.Location = new System.Drawing.Point(12, 332);
            this.jd_grp_auto.Name = "jd_grp_auto";
            this.jd_grp_auto.Size = new System.Drawing.Size(1074, 51);
            this.jd_grp_auto.TabIndex = 4;
            this.jd_grp_auto.TabStop = false;
            this.jd_grp_auto.Text = "자동매매 시작/중지";
            this.jd_grp_auto.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // jd_btn_stop
            // 
            this.jd_btn_stop.Location = new System.Drawing.Point(560, 20);
            this.jd_btn_stop.Name = "jd_btn_stop";
            this.jd_btn_stop.Size = new System.Drawing.Size(500, 23);
            this.jd_btn_stop.TabIndex = 1;
            this.jd_btn_stop.Text = "자동매매 중지";
            this.jd_btn_stop.UseVisualStyleBackColor = true;
            this.jd_btn_stop.Click += new System.EventHandler(this.button6_Click);
            // 
            // jd_btn_start
            // 
            this.jd_btn_start.Location = new System.Drawing.Point(20, 20);
            this.jd_btn_start.Name = "jd_btn_start";
            this.jd_btn_start.Size = new System.Drawing.Size(534, 23);
            this.jd_btn_start.TabIndex = 0;
            this.jd_btn_start.Text = "자동매매 시작";
            this.jd_btn_start.UseVisualStyleBackColor = true;
            this.jd_btn_start.Click += new System.EventHandler(this.button5_Click);
            // 
            // jd_grp_msg
            // 
            this.jd_grp_msg.Controls.Add(this.jd_txt_msg);
            this.jd_grp_msg.Location = new System.Drawing.Point(15, 389);
            this.jd_grp_msg.Name = "jd_grp_msg";
            this.jd_grp_msg.Size = new System.Drawing.Size(551, 156);
            this.jd_grp_msg.TabIndex = 5;
            this.jd_grp_msg.TabStop = false;
            this.jd_grp_msg.Text = "메시지 로그";
            this.jd_grp_msg.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // jd_txt_msg
            // 
            this.jd_txt_msg.BackColor = System.Drawing.Color.Black;
            this.jd_txt_msg.ForeColor = System.Drawing.Color.Lime;
            this.jd_txt_msg.Location = new System.Drawing.Point(6, 20);
            this.jd_txt_msg.Multiline = true;
            this.jd_txt_msg.Name = "jd_txt_msg";
            this.jd_txt_msg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.jd_txt_msg.Size = new System.Drawing.Size(539, 130);
            this.jd_txt_msg.TabIndex = 0;
            this.jd_txt_msg.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // jd_grp_err
            // 
            this.jd_grp_err.Controls.Add(this.jd_txt_err);
            this.jd_grp_err.Location = new System.Drawing.Point(572, 394);
            this.jd_grp_err.Name = "jd_grp_err";
            this.jd_grp_err.Size = new System.Drawing.Size(515, 156);
            this.jd_grp_err.TabIndex = 6;
            this.jd_grp_err.TabStop = false;
            this.jd_grp_err.Text = "오류 로그";
            this.jd_grp_err.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // jd_txt_err
            // 
            this.jd_txt_err.BackColor = System.Drawing.Color.Black;
            this.jd_txt_err.ForeColor = System.Drawing.Color.Yellow;
            this.jd_txt_err.Location = new System.Drawing.Point(6, 15);
            this.jd_txt_err.Multiline = true;
            this.jd_txt_err.Name = "jd_txt_err";
            this.jd_txt_err.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.jd_txt_err.Size = new System.Drawing.Size(494, 130);
            this.jd_txt_err.TabIndex = 1;
            this.jd_txt_err.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // jd_ssr_bottom
            // 
            this.jd_ssr_bottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jd_ssr_label1});
            this.jd_ssr_bottom.Location = new System.Drawing.Point(0, 553);
            this.jd_ssr_bottom.Name = "jd_ssr_bottom";
            this.jd_ssr_bottom.Size = new System.Drawing.Size(1098, 22);
            this.jd_ssr_bottom.TabIndex = 7;
            this.jd_ssr_bottom.Text = "statusStrip1";
            this.jd_ssr_bottom.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // jd_ssr_label1
            // 
            this.jd_ssr_label1.Name = "jd_ssr_label1";
            this.jd_ssr_label1.Size = new System.Drawing.Size(157, 17);
            this.jd_ssr_label1.Text = "ats에 오신 것을 환영합니다.";
            this.jd_ssr_label1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // seq
            // 
            this.seq.HeaderText = "순번";
            this.seq.Name = "seq";
            this.seq.Width = 60;
            // 
            // jongmok_cd
            // 
            this.jongmok_cd.HeaderText = "종목코드";
            this.jongmok_cd.Name = "jongmok_cd";
            this.jongmok_cd.Width = 80;
            // 
            // jongmok_nm
            // 
            this.jongmok_nm.HeaderText = "종목명";
            this.jongmok_nm.Name = "jongmok_nm";
            this.jongmok_nm.Width = 120;
            // 
            // priority
            // 
            this.priority.HeaderText = "우선순위";
            this.priority.Name = "priority";
            this.priority.Width = 80;
            // 
            // buy_amt
            // 
            this.buy_amt.HeaderText = "매수금액";
            this.buy_amt.Name = "buy_amt";
            this.buy_amt.Width = 120;
            // 
            // buy_price
            // 
            this.buy_price.HeaderText = "매수가";
            this.buy_price.Name = "buy_price";
            this.buy_price.Width = 120;
            // 
            // target_price
            // 
            this.target_price.HeaderText = "목표가";
            this.target_price.Name = "target_price";
            this.target_price.Width = 120;
            // 
            // cut_loss_price
            // 
            this.cut_loss_price.HeaderText = "손절가";
            this.cut_loss_price.Name = "cut_loss_price";
            this.cut_loss_price.Width = 120;
            // 
            // buy_trd_yn
            // 
            this.buy_trd_yn.HeaderText = "매수여부";
            this.buy_trd_yn.Items.AddRange(new object[] {
            "Y",
            "N"});
            this.buy_trd_yn.Name = "buy_trd_yn";
            this.buy_trd_yn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.buy_trd_yn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.buy_trd_yn.Width = 80;
            // 
            // sell_trd_yn
            // 
            this.sell_trd_yn.HeaderText = "매도여부";
            this.sell_trd_yn.Items.AddRange(new object[] {
            "Y",
            "N"});
            this.sell_trd_yn.Name = "sell_trd_yn";
            this.sell_trd_yn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sell_trd_yn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.sell_trd_yn.Width = 80;
            // 
            // check
            // 
            this.check.HeaderText = "체크";
            this.check.Name = "check";
            this.check.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.check.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.check.Width = 60;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 575);
            this.Controls.Add(this.jd_ssr_bottom);
            this.Controls.Add(this.jd_grp_err);
            this.Controls.Add(this.jd_kw_openAPI);
            this.Controls.Add(this.jd_grp_msg);
            this.Controls.Add(this.jd_grp_auto);
            this.Controls.Add(this.jd_grp_trd);
            this.Controls.Add(this.jd_grp_user_info);
            this.Controls.Add(this.jd_mns_top);
            this.MainMenuStrip = this.jd_mns_top;
            this.Name = "Form1";
            this.Text = "ats";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.jd_kw_openAPI)).EndInit();
            this.jd_mns_top.ResumeLayout(false);
            this.jd_mns_top.PerformLayout();
            this.jd_grp_user_info.ResumeLayout(false);
            this.jd_grp_user_info.PerformLayout();
            this.jd_grp_trd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.jd_dgv_main)).EndInit();
            this.jd_grp_auto.ResumeLayout(false);
            this.jd_grp_msg.ResumeLayout(false);
            this.jd_grp_msg.PerformLayout();
            this.jd_grp_err.ResumeLayout(false);
            this.jd_grp_err.PerformLayout();
            this.jd_ssr_bottom.ResumeLayout(false);
            this.jd_ssr_bottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxKHOpenAPILib.AxKHOpenAPI jd_kw_openAPI;
        private System.Windows.Forms.MenuStrip jd_mns_top;
        private System.Windows.Forms.ToolStripMenuItem jd_mns_login;
        private System.Windows.Forms.ToolStripMenuItem jd_mns_logout;
        private System.Windows.Forms.GroupBox jd_grp_user_info;
        private System.Windows.Forms.TextBox jd_txt_user_id;
        private System.Windows.Forms.Label jd_lbl_user_id;
        private System.Windows.Forms.ComboBox jd_cmb_acc_no;
        private System.Windows.Forms.Label jd_lbl_acc_no;
        private System.Windows.Forms.GroupBox jd_grp_trd;
        private System.Windows.Forms.Button jd_btn_remove;
        private System.Windows.Forms.Button jd_btn_update;
        private System.Windows.Forms.Button jd_btn_insert;
        private System.Windows.Forms.Button jd_btn_search;
        private System.Windows.Forms.DataGridView jd_dgv_main;
        private System.Windows.Forms.GroupBox jd_grp_auto;
        private System.Windows.Forms.Button jd_btn_stop;
        private System.Windows.Forms.Button jd_btn_start;
        private System.Windows.Forms.GroupBox jd_grp_msg;
        private System.Windows.Forms.TextBox jd_txt_msg;
        private System.Windows.Forms.GroupBox jd_grp_err;
        private System.Windows.Forms.TextBox jd_txt_err;
        private System.Windows.Forms.StatusStrip jd_ssr_bottom;
        private System.Windows.Forms.ToolStripStatusLabel jd_ssr_label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn seq;
        private System.Windows.Forms.DataGridViewTextBoxColumn jongmok_cd;
        private System.Windows.Forms.DataGridViewTextBoxColumn jongmok_nm;
        private System.Windows.Forms.DataGridViewTextBoxColumn priority;
        private System.Windows.Forms.DataGridViewTextBoxColumn buy_amt;
        private System.Windows.Forms.DataGridViewTextBoxColumn buy_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn target_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn cut_loss_price;
        private System.Windows.Forms.DataGridViewComboBoxColumn buy_trd_yn;
        private System.Windows.Forms.DataGridViewComboBoxColumn sell_trd_yn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn check;
    }
}

