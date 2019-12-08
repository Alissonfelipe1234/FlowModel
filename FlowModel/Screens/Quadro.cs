﻿using FlowModel.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FlowModel
{
    public partial class Quadro : Form
    {
        private List<Desenho> figuras;
        private Desenho selecionado;

        private const int tamanhoX = 1015;
        private const int tamanhoY = 600;

        private int click;
        private bool BatterySaver, achou, arrastandoFigura, desenhandoRelacionamento, desenhandoEntidade, desenhandoEspecializacao, desenhandoPadronizacao, desenhandoAtributo;
        private int envolvidos;

        private Bitmap BmpImagem;
        private Graphics GrpImage;

        public Quadro()
        {
            InitializeComponent();
        }

        private void EditPanel_Load(object sender, EventArgs e)
        {
            BmpImagem = new Bitmap(tamanhoX, tamanhoY);
            pn_edit.BackgroundImage = BmpImagem;

            GrpImage = Graphics.FromImage(BmpImagem);
            GrpImage.Clear(Color.White);

            figuras = new List<Desenho>();
            click = 0;
            envolvidos = 0;
            desenhandoRelacionamento = false;
            desenhandoEspecializacao = false;
            desenhandoPadronizacao = false;
            desenhandoEntidade = false;
            desenhandoAtributo = false;
            arrastandoFigura = false;
            BatterySaver = false;
            selecionado = null;
        }


        public Graphics GetGraphics()
        {
            return this.GrpImage;
        }
        public Bitmap GetBmp()
        {
            return this.BmpImagem;
        }

        private void Btn_entidade_Click(object sender, EventArgs e)
        {
            desenhandoEntidade = true;
            desenhandoRelacionamento = false;
            desenhandoPadronizacao = false;
            desenhandoEspecializacao = false;
            desenhandoAtributo = false;
            InfoOqueFazer.Text = "clique no quadro para desenhar";
        }

        private void Btn_relacionamento_Click(object sender, EventArgs e)
        {
            desenhandoEntidade = false;
            desenhandoRelacionamento = true;
            desenhandoEspecializacao = false;
            desenhandoPadronizacao = false;
            desenhandoAtributo = false;
            InfoOqueFazer.Text = "Selecione uma entidade";
        }

        private void Btn_padrao_Click(object sender, EventArgs e)
        {
            desenhandoPadronizacao = true;
            desenhandoEntidade = false;
            desenhandoRelacionamento = false;
            desenhandoEspecializacao = false;
            desenhandoAtributo = false;
            InfoOqueFazer.Text = "Selecione uma entidade";
        }

        private void Btn_atributo_Click(object sender, EventArgs e)
        {
            desenhandoEspecializacao = false;
            desenhandoRelacionamento = false;
            desenhandoEntidade = false;
            desenhandoPadronizacao = false;
            desenhandoAtributo = true;
            InfoOqueFazer.Text = "clique em um objeto";
        }

        private void Btn_heranca_Click(object sender, EventArgs e)
        {
            desenhandoEspecializacao = true;
            desenhandoRelacionamento = false;
            desenhandoEntidade = false;
            desenhandoPadronizacao = false;
            desenhandoAtributo = false;
            InfoOqueFazer.Text = "Selecione uma entidade";
        }

        private void NomeEntidade_TextChanged(object sender, EventArgs e)
        {

            this.selecionado.SetName(NomeEntidade.Text);
            if (this.NomeEntidade.TextLength > 8)
            {
                GrpImage.Clear(Color.White);
                foreach (Desenho d in figuras)
                    d.SeDesenhe(GrpImage);
                pn_edit.Refresh();
            }
            else
            {
                this.selecionado.SeDesenhe(GrpImage);
                pn_edit.Refresh();
            }
        }

        private void TxtEntidadeX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtEntidadeX.Text != "" && txtEntidadeX.Text != "-")
                {
                    Entidade mudada = (Entidade)this.selecionado;
                    mudada.SetX(int.Parse(txtEntidadeX.Text));

                    foreach (Desenho d in figuras)
                    {
                        if (d.QuemSou() == "Atributo")
                        {
                            Atributo temQueMudar = (Atributo)d;
                            if (temQueMudar.GetProprietario() == mudada)
                            {
                                temQueMudar.SetX(int.Parse(txtEntidadeX.Text));
                            }
                        }
                    }
                    GrpImage.Clear(Color.White);
                    foreach (Desenho d in figuras)
                        d.SeDesenhe(GrpImage);
                    pn_edit.Refresh();
                }
            }
            catch (Exception)
            {
                txtEntidadeX.Text = this.selecionado.GetX().ToString();
            }
        }

        private void TxtEntidadeY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtEntidadeX.Text != "" && txtEntidadeX.Text != "-")
                {
                    Entidade mudada = (Entidade)this.selecionado;
                    mudada.SetY(int.Parse(txtEntidadeY.Text));

                    foreach (Desenho d in figuras)
                    {
                        if (d.QuemSou() == "Atributo")
                        {
                            Atributo temQueMudar = (Atributo)d;
                            if (temQueMudar.GetProprietario() == mudada)
                            {
                                switch (temQueMudar.GetTipo())
                                {
                                    case "Comum":
                                        temQueMudar.SetY(int.Parse(txtEntidadeY.Text) + 50);
                                        break;
                                    default:
                                        temQueMudar.SetY(int.Parse(txtEntidadeY.Text));
                                        break;
                                }

                            }
                        }
                    }
                    GrpImage.Clear(Color.White);
                    foreach (Desenho d in figuras)
                        d.SeDesenhe(GrpImage);
                    pn_edit.Refresh();
                }
            }
            catch (Exception)
            {
                txtEntidadeX.Text = this.selecionado.GetX().ToString();
            }
        }

        private void ComboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Atributo Aselecionado = (Atributo)this.selecionado;
            Aselecionado.SetDado(cbTipoAtributo.SelectedIndex);
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if ((e.KeyCode.Equals(Keys.Back) || e.KeyCode.Equals(Keys.Delete)) && this.selecionado != null)
            {
                figuras.Remove(this.selecionado);
                GrpImage.Clear(Color.White);

                foreach (Desenho d in figuras)
                {
                    d.SeDesenhe(this.GrpImage);
                }
                this.pn_edit.Refresh();
                this.selecionado = null;
            }
        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void NomeAtributo_TextChanged(object sender, EventArgs e)
        {
            this.selecionado.SetName(NomeAtributo.Text);
            GrpImage.Clear(Color.White);
            foreach (Desenho figura in figuras)
            {
                figura.SeDesenhe(GrpImage);
            }
            pn_edit.Refresh();
        }

        private void CbDonoAtributo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Desenho novoDono = (Desenho)cbDonoAtributo.SelectedItem;
            Atributo Aselecionado = (Atributo)this.selecionado;
            Aselecionado.SetProprietario(novoDono);

            if (Aselecionado.GetTipo() == "Comum")
            {
                Aselecionado.SetX(novoDono.GetX());
                Aselecionado.SetY(novoDono.GetY() + 50);
            }
            else if (Aselecionado.GetTipo() == "Opcional")
            {
                Aselecionado.SetX(novoDono.GetX() + 86);
                Aselecionado.SetY(novoDono.GetY() - 40);
            }
            else
            {
                Aselecionado.SetX(novoDono.GetX());
                Aselecionado.SetY(novoDono.GetY() - 40);
            }

            GrpImage.Clear(Color.White);
            foreach (Desenho d in figuras)
                d.SeDesenhe(GrpImage);
            pn_edit.Refresh();
        }

        private void TxtCardAtributoMin_TextChanged(object sender, EventArgs e)
        {
            Atributo Aselecionado = (Atributo)this.selecionado;
            switch (txtCardAtributoMin.Text)
            {
                case "0":
                    GrpImage.Clear(Color.White);
                    switch (Aselecionado.GetTipo())
                    {
                        case "Primario":
                            Aselecionado.PrimarioToComum();
                            Aselecionado.ComumToOpcional();
                            Primario.Checked = false;
                            break;
                        case "Comum":
                            Aselecionado.ComumToOpcional();
                            break;
                    }
                    Aselecionado.SetCardMin(0);
                    foreach (Desenho f in figuras)
                        f.SeDesenhe(GrpImage);
                    pn_edit.Refresh();
                    break;
                case "1":
                    GrpImage.Clear(Color.White);
                    if (Aselecionado.GetTipo() == "Opcional")
                    {
                        Aselecionado.OpcionalToComum();
                    }
                    Aselecionado.SetCardMin(1);
                    foreach (Desenho f in figuras)
                        f.SeDesenhe(GrpImage);
                    pn_edit.Refresh();
                    break;
            }

        }

        private void TxtCardAtributoMax_TextChanged(object sender, EventArgs e)
        {
            Atributo Aselecionado = (Atributo)this.selecionado;
            if (txtCardAtributoMax.Text.ToUpper().Equals("N"))
            {
                txtCardAtributoMax.Text = "N";
                if (Aselecionado.GetPropriedade()[4] == 1)
                {
                    Aselecionado.SetName(Aselecionado.GetName() + " " + Aselecionado.GetPropriedade()[3] + ", N");
                    Aselecionado.SeDesenhe(GrpImage);
                    pn_edit.Refresh();
                    Aselecionado.SetCardMax(2);
                }
            }
            if (txtCardAtributoMax.Text.Equals("1"))
            {
                if (Aselecionado.GetPropriedade()[4] == 2)
                {
                    Aselecionado.SetName(Aselecionado.GetName().Split(' ')[0]);
                    GrpImage.Clear(Color.White);
                    foreach (Desenho f in figuras)
                    {
                        f.SeDesenhe(GrpImage);
                        pn_edit.Refresh();
                    }
                    Aselecionado.SetCardMax(1);
                }
            }
        }

        private void Primario_CheckedChanged(object sender, EventArgs e)
        {
            Atributo Aselecionado = (Atributo)this.selecionado;
            if (Primario.Checked == true && Aselecionado.GetPropriedade()[0] != 1)
            {
                txtCardAtributoMin.Text = "1";
                Aselecionado.ComumToPrimario();
                GrpImage.Clear(Color.White);
                foreach (Desenho g in figuras)
                    g.SeDesenhe(GrpImage);
                pn_edit.Refresh();
            }
            else
            {
                if (Aselecionado.GetPropriedade()[0] == 1)
                {
                    Aselecionado.PrimarioToComum();
                    GrpImage.Clear(Color.White);
                    foreach (Desenho g in figuras)
                        g.SeDesenhe(GrpImage);
                    pn_edit.Refresh();
                }
            }
        }

        private void Derivado_CheckedChanged(object sender, EventArgs e)
        {
            Atributo Aselecionado = (Atributo)this.selecionado;

            if (Derivado.Checked == true)
            {
                cbDerivado.Visible = true;
                cbDerivado.Items.Clear();
                foreach (Desenho k in figuras)
                {
                    if (k.QuemSou() == "Atributo")
                    {
                        cbDerivado.Items.Add(k);
                    }
                    if (Aselecionado.GetDerivado() == k)
                    {
                        cbDerivado.SelectedItem = k;
                    }
                }
            }
            else
            {
                cbDerivado.Visible = false;
            }
        }

        private void CbDerivado_SelectedIndexChanged(object sender, EventArgs e)
        {
            Atributo Aselecionado = (Atributo)this.selecionado;
            Aselecionado.SetDerivado((Atributo)cbDerivado.SelectedItem);
        }

        private void Pn_edit_MouseUp(object sender, MouseEventArgs e)
        {
            arrastandoFigura = false;
            if (BatterySaver)
            {
                GrpImage.Clear(Color.White);
                foreach (Desenho d in figuras)
                    d.SeDesenhe(GrpImage);
                pn_edit.Refresh();
            }
        }

        private void NomeRelacionamento_TextChanged(object sender, EventArgs e)
        {
            this.selecionado.SetName(NomeRelacionamento.Text);
            GrpImage.Clear(Color.White);
            foreach (Desenho f in figuras)
                f.SeDesenhe(GrpImage);
            pn_edit.Refresh();
        }

        private void RelacionamentoX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (RelacionamentoX.Text != "" && RelacionamentoX.Text != "-")
                {
                    this.selecionado.SetX(int.Parse(RelacionamentoX.Text));
                    GrpImage.Clear(Color.White);
                    foreach (Desenho d in figuras)
                        d.SeDesenhe(GrpImage);
                    pn_edit.Refresh();
                }
            }
            catch (Exception)
            {
                RelacionamentoX.Text = this.selecionado.GetX().ToString();
            }
        }

        private void RelacionamentoY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (RelacionamentoY.Text != "" && RelacionamentoY.Text != "-")
                {
                    this.selecionado.SetY(int.Parse(RelacionamentoY.Text));
                    GrpImage.Clear(Color.White);
                    foreach (Desenho d in figuras)
                        d.SeDesenhe(GrpImage);
                    pn_edit.Refresh();
                }
            }
            catch (Exception)
            {
                RelacionamentoY.Text = this.selecionado.GetX().ToString();
            }
        }

        private void EntidadeDono1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Entidade ex = (Entidade)EntidadeDono1.SelectedItem;
            Relacionamento rel = (Relacionamento)this.selecionado;
            rel.getCards()[0].SetX(ex.GetX());
            rel.getCards()[0].SetY(ex.GetY() + 50);

            rel.setEntidade(0, ex);
            foreach (Desenho d in figuras)
                d.SeDesenhe(GrpImage);
            pn_edit.Refresh();
        }

        private void EntidadeDono2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Entidade ex = (Entidade)EntidadeDono1.SelectedItem;
            Relacionamento rel = (Relacionamento)this.selecionado;
            rel.getCards()[1].SetX(ex.GetX());
            rel.getCards()[1].SetY(ex.GetY() + 50);

            rel.setEntidade(1, ex);
            foreach (Desenho d in figuras)
                d.SeDesenhe(GrpImage);
            pn_edit.Refresh();
        }

        private void EntidadeDono3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Entidade ex = (Entidade)EntidadeDono1.SelectedItem;
            Relacionamento rel = (Relacionamento)this.selecionado;
            rel.getCards()[2].SetX(ex.GetX());
            rel.getCards()[2].SetY(ex.GetY() + 50);

            rel.setEntidade(2, ex);
            GrpImage.Clear(Color.White);
            foreach (Desenho d in figuras)
                d.SeDesenhe(GrpImage);
            pn_edit.Refresh();
        }
        private void EditPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<Entidade> entidades = new List<Entidade>();
            List<Relacionamento> relacionamentos = new List<Relacionamento>();
            List<Atributo> atributos = new List<Atributo>();
            List<Padronizacao> padronizacaos = new List<Padronizacao>();
            List<Especializacao> especializacaos = new List<Especializacao>();

            foreach (Desenho i in figuras)
            {
                switch (i.QuemSou())
                {
                    case "Entidade":
                        entidades.Add((Entidade)i);
                        break;
                    case "Relacionamento":
                        relacionamentos.Add((Relacionamento)i);
                        break;
                    case "Atributo":
                        atributos.Add((Atributo)i);
                        break;
                    case "Padronizacao":
                        padronizacaos.Add((Padronizacao)i);
                        break;
                    case "Especializacao":
                        especializacaos.Add((Especializacao)i);
                        break;
                }
            }
            /*
            string connString = @"Host=127.0.0.1;Username=postgres;Password=230276;Database=Interdiciplinar";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                //CASO DE ERRO NA HORA DE FECHAR É PQ NÃO EXISTE O BANCO (string acima pode ser alterada)
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    string nomeProject = Nome.Text;
                    cmd.CommandText = "Drop table PROJETO CASCADE";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Drop table Entidade CASCADE";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Drop table Relacionamento CASCADE";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Drop table Cardinalidade CASCADE";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Drop table Atributo CASCADE";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Drop table Padronizacao CASCADE";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Drop table Especializacao CASCADE";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "COMMIT;";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "CREATE TABLE PROJETO" +
                    "(" +
                    "	nome varchar(255)" +
                    ");" +
                    "CREATE TABLE Entidade(" +
                    "    	ID serial Primary Key," +
                    "    	Nome varchar(255) NOT NULL," +
                    "    	x integer NOT NULL," +
                    "    	y integer NOT NULL," +
                    "		qtdAtributos integer NOT NULL" +
                    ");" +
                    "" +
                    "CREATE TABLE Relacionamento(" +
                    "    	ID serial Primary Key," +
                    "    	Nome varchar(255) NOT NULL," +
                    "	x integer NOT NULL," +
                    "	y integer NOT NULL," +
                    "	qtdAtributos integer NOT NULL," +
                    "	qtdEnv integer NOT NULL," +
                    "	idEntidade1 integer NOT NULL," +
                    "	idEntidade2 integer," +
                    "	idEntidade3 integer" +
                    ");" +
                    "" +
                    "CREATE TABLE Cardinalidade(" +
                    "    	ID serial Primary Key," +
                    "    	cardMin varchar(1) NOT NULL," +
                    "	cardMax varchar(1) NOT NULL," +
                    "	x integer NOT NULL," +
                    "	y integer NOT NULL," +
                    "	IdRelacionamento integer," +
                    "	FOREIGN KEY (IdRelacionamento) REFERENCES Relacionamento(ID)" +
                    ");" +
                    "" +
                    "CREATE TABLE Atributo(	" +
                    "	ID serial Primary Key," +
                    "	Nome varchar(255) NOT NULL," +
                    "	x integer NOT NULL," +
                    "	y integer NOT NULL," +
                    "	indice integer NOT NULL," +
                    "	qtdAtributos integer NOT NULL," +
                    "	dado integer NOT NULL," +
                    "	propriedades varchar(20) NOT NULL," +
                    "	ID_Proprietario integer NOT NULL," +
                    "	Tipo_Proprietario varchar(15) NOT NULL" +
                    ");" +
                    "" +
                    "CREATE TABLE Padronizacao(" +
                    "    	ID serial Primary Key," +
                    "    	Nome varchar(255) NOT NULL," +
                    "    	x integer NOT NULL," +
                    "    	y integer NOT NULL," +
                    "	Padrao integer NOT NULL," +
                    "	List varchar(60) NOT NULL" +
                    ");" +
                    "CREATE TABLE Especializacao(" +
                    "    	ID serial Primary Key," +
                    "    	Nome varchar(255) NOT NULL," +
                    "    	x integer NOT NULL," +
                    "    	y integer NOT NULL," +
                    "	Especial integer NOT NULL," +
                    "	List varchar(60) NOT NULL" +
                    ");";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "COMMIT;";
                    cmd.ExecuteNonQuery();

                }
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    string nomeProject = Nome.Text;
                    cmd.CommandText = "INSERT INTO PROJETO VALUES(@nome)";

                    cmd.Parameters.AddWithValue("nome", nomeProject);
                    cmd.ExecuteNonQuery();
                }
                string nome = "";
                int x = 0;
                int y = 0;
                int qtd = 0;
                int qtdEnv = 0;

                foreach (Entidade salva in entidades)
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        nome = salva.getName();
                        x = salva.getX();
                        y = salva.getY();
                        qtd = salva.getQtdAtributos();

                        cmd.CommandText = "INSERT INTO Entidade (Nome, x, y, qtdAtributos)" +
                            " VALUES(@nome, @x, @y, @qtdAtributos)";

                        cmd.Parameters.AddWithValue("nome", nome);
                        cmd.Parameters.AddWithValue("x", x);
                        cmd.Parameters.AddWithValue("y", y);
                        cmd.Parameters.AddWithValue("qtdAtributos", qtd);
                        cmd.ExecuteNonQuery();
                    }
                }
                int i = 0;
                foreach (Relacionamento salva in relacionamentos)
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {

                        cmd.Connection = conn;

                        nome = salva.getName();
                        x = salva.getX();
                        y = salva.getY();
                        qtd = salva.getQtdAtributos();
                        qtdEnv = salva.getQtdEnvolvidos();
                        List<Entidade> envs = salva.getEnvolvidos();
                        if (qtdEnv == 1)
                        {
                            int entidade1 = entidades.IndexOf(envs[0]);

                            cmd.CommandText = "INSERT INTO Relacionamento (Nome, x, y, qtdAtributos, qtdEnv, idEntidade1)" +
                                " VALUES(@nome, @x, @y, @qtdAtributos, @qtdEnv, @idEntidade1)";

                            cmd.Parameters.AddWithValue("nome", nome);
                            cmd.Parameters.AddWithValue("x", x);
                            cmd.Parameters.AddWithValue("y", y);
                            cmd.Parameters.AddWithValue("qtdAtributos", qtd);
                            cmd.Parameters.AddWithValue("qtdEnv", qtdEnv);
                            cmd.Parameters.AddWithValue("idEntidade1", entidade1);
                            cmd.ExecuteNonQuery();
                        }
                        else if (qtdEnv == 2)
                        {
                            int entidade1 = entidades.IndexOf(envs[0]);
                            int entidade2 = entidades.IndexOf(envs[1]);

                            cmd.CommandText = "INSERT INTO Relacionamento (Nome, x, y, qtdAtributos, qtdEnv, idEntidade1, idEntidade2)" +
                                " VALUES(@nome, @x, @y, @qtdAtributos, @qtdEnv, @idEntidade1, @idEntidade2)";

                            cmd.Parameters.AddWithValue("nome", nome);
                            cmd.Parameters.AddWithValue("x", x);
                            cmd.Parameters.AddWithValue("y", y);
                            cmd.Parameters.AddWithValue("qtdAtributos", qtd);
                            cmd.Parameters.AddWithValue("qtdEnv", qtdEnv);
                            cmd.Parameters.AddWithValue("idEntidade1", entidade1);
                            cmd.Parameters.AddWithValue("idEntidade2", entidade2);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            int entidade1 = entidades.IndexOf(envs[0]);
                            int entidade2 = entidades.IndexOf(envs[1]);
                            int entidade3 = entidades.IndexOf(envs[2]);

                            cmd.CommandText = "INSERT INTO Relacionamento (Nome, x, y, qtdAtributos, qtdEnv, idEntidade1, idEntidade2, idEntidade3)" +
                                " VALUES(@nome, @x, @y, @qtdAtributos, @qtdEnv, @idEntidade1, @idEntidade2, @idEntidade3)";

                            cmd.Parameters.AddWithValue("nome", nome);
                            cmd.Parameters.AddWithValue("x", x);
                            cmd.Parameters.AddWithValue("y", y);
                            cmd.Parameters.AddWithValue("qtdAtributos", qtd);
                            cmd.Parameters.AddWithValue("qtdEnv", qtdEnv);
                            cmd.Parameters.AddWithValue("idEntidade1", entidade1);
                            cmd.Parameters.AddWithValue("idEntidade2", entidade2);
                            cmd.Parameters.AddWithValue("idEntidade3", entidade3);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    foreach (Cardinalidade card in salva.getCards())
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;

                            string cardMin = card.getCardMin();
                            string cardMax = card.getCardMax();

                            x = card.getX();
                            y = card.getY();

                            int idRelacionamento = i;
                            List<Entidade> envs = salva.getEnvolvidos();
                            if (qtdEnv == 1)
                            {
                                int entidade1 = 0;
                                entidade1 = entidades.IndexOf(envs[0]);

                                cmd.CommandText = "INSERT INTO Cardinalidade (cardMin, cardMax,x, y, IdRelacionamento)" +
                                    " VALUES(@cardMin, @cardMax, @x, @y, @IdRelacionamento)";

                                cmd.Parameters.AddWithValue("cardMin", cardMin);
                                cmd.Parameters.AddWithValue("cardMax", cardMax);
                                cmd.Parameters.AddWithValue("x", x);
                                cmd.Parameters.AddWithValue("y", y);
                                cmd.Parameters.AddWithValue("IdRelacionamento", idRelacionamento);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    i++;
                }
                foreach (Padronizacao p in padronizacaos)
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        nome = p.getName();
                        x = p.getX();
                        y = p.getY();
                        int padrao = entidades.IndexOf(p.getEntidadePadrao());
                        string list = "";
                        foreach (Entidade g in p.getEntidades())
                        {
                            list += entidades.IndexOf(g);
                            list += ";";
                        }
                        cmd.CommandText = "INSERT INTO Padronizacao (Nome, x, y, Padrao, List)" +
                            " VALUES(@nome, @x, @y, @Padrao, @List)";

                        cmd.Parameters.AddWithValue("nome", nome);
                        cmd.Parameters.AddWithValue("x", x);
                        cmd.Parameters.AddWithValue("y", y);
                        cmd.Parameters.AddWithValue("Padrao", padrao);
                        cmd.Parameters.AddWithValue("List", list);
                        cmd.ExecuteNonQuery();
                    }
                }
                foreach (Especializacao t in especializacaos)
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        nome = t.getName();
                        x = t.getX();
                        y = t.getY();
                        int espec = entidades.IndexOf(t.getEntidadeEspecializada());
                        string list = "";
                        foreach (Entidade g in t.getEntidades())
                        {
                            list += entidades.IndexOf(g);
                            list += ";";
                        }
                        cmd.CommandText = "INSERT INTO Especializacao (Nome, x, y, Especial, List)" +
                            " VALUES(@nome, @x, @y, @Especial, @List)";

                        cmd.Parameters.AddWithValue("nome", nome);
                        cmd.Parameters.AddWithValue("x", x);
                        cmd.Parameters.AddWithValue("y", y);
                        cmd.Parameters.AddWithValue("Especial", espec);
                        cmd.Parameters.AddWithValue("List", list);
                        cmd.ExecuteNonQuery();
                    }
                }
                foreach (Atributo a in atributos)
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        nome = a.getName();
                        x = a.getX();
                        y = a.getY();
                        int indice = a.getIndice();
                        int qtdAtributos = a.getQtdAtributos();
                        int dado = a.getIntDado();
                        string propriedades = "";
                        foreach (int prop in a.getPropriedade())
                        {
                            propriedades += prop + ";";
                        }
                        string Tipo_Proprietario = a.getProprietario().QuemSou();
                        int ID_Proprietario = 0;
                        switch (Tipo_Proprietario)
                        {
                            case "Entidade":
                                ID_Proprietario = entidades.IndexOf((Entidade)a.getProprietario());
                                break;
                            case "Relacionamento":
                                ID_Proprietario = relacionamentos.IndexOf((Relacionamento)a.getProprietario());
                                break;
                            case "Atributo":
                                ID_Proprietario = atributos.IndexOf((Atributo)a.getProprietario());
                                break;
                        }

                        cmd.CommandText = "INSERT INTO Atributo (Nome, x, y, indice, qtdAtributos, dado, propriedades, ID_Proprietario, Tipo_Proprietario)" +
                            " VALUES(@nome, @x, @y, @indice, @qtdAtributos, @dado, @propriedades, @ID_Proprietario, @Tipo_Proprietario)";

                        cmd.Parameters.AddWithValue("nome", nome);
                        cmd.Parameters.AddWithValue("x", x);
                        cmd.Parameters.AddWithValue("y", y);
                        cmd.Parameters.AddWithValue("indice", indice);

                        cmd.Parameters.AddWithValue("qtdAtributos", qtdAtributos);
                        cmd.Parameters.AddWithValue("dado", dado);
                        cmd.Parameters.AddWithValue("propriedades", propriedades);
                        cmd.Parameters.AddWithValue("ID_Proprietario", ID_Proprietario);
                        cmd.Parameters.AddWithValue("Tipo_Proprietario", Tipo_Proprietario);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            InputMessage("Exportação concluida", "Modelo salvo com sucesso!");
            */

        }

        private void GerarSQL_Click(object sender, EventArgs e)
        {
            string sqlGerado = Gerador.SQL(Nome.Text, figuras);
            this.Hide();
            this.Visible = false;
            using (ResultadoSQL tela = new ResultadoSQL(sqlGerado, this))
            {
                tela.Show();
                tela.Visible = true;
            }
        }

        private void Salvar_Click(object sender, EventArgs e)
        {
            Banco.SalvarProjeto(figuras, Nome.Text);
            InputMessage("Exportação concluida", "Modelo salvo com sucesso!");
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar1.Value == 0)
            {
                BatterySaver = false;
                trackBar1.BackColor = Color.Snow;
                this.BackColor = Color.Snow;
            }
            else
            {

                BatterySaver = true;
                trackBar1.BackColor = Color.DarkGray;
                this.BackColor = Color.DarkGray;
            }
        }

        private void Pn_edit_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (Desenho d in figuras)
            {
                if (d.GetArea(e.X, e.Y))
                {
                    this.selecionado = d;
                    arrastandoFigura = true;
                }
                if (d.QuemSou().Equals("Relacionamento"))
                {
                    Relacionamento r = (Relacionamento)d;
                    foreach (Cardinalidade c in r.getCards())
                    {
                        if (c.GetArea(e.X, e.Y))
                        {
                            this.selecionado = c;
                            arrastandoFigura = true;
                            break;
                        }
                    }
                }
                if (arrastandoFigura)
                    break;
            }

        }

        private void Pn_edit_MouseMove(object sender, MouseEventArgs e)
        {
            Info.Text = e.X + "," + e.Y;
            if (arrastandoFigura)
            {
                this.selecionado.SetX(e.X);
                this.selecionado.SetY(e.Y);

                if (!BatterySaver)
                {
                    GrpImage.Clear(Color.White);
                    foreach (Desenho f in figuras)
                    {
                        f.SeDesenhe(GrpImage);
                    }
                    pn_edit.Refresh();
                }
            }
        }

        private void Pn_edit_MouseClick(object sender, MouseEventArgs e)
        {
            InfoOqueFazer.Text = "";
            string value;
            BoxEntidade.Visible = false;
            BoxAtributo.Visible = false;
            BoxRelacionamento.Visible = false;
            if (!desenhandoAtributo && !desenhandoEntidade && !desenhandoEspecializacao && !desenhandoPadronizacao && !desenhandoRelacionamento)
            {

                for (int i = 0; i < figuras.Count; i++)
                {
                    if (figuras[i].GetArea(e.X, e.Y))
                    {
                        this.selecionado = figuras[i];
                        switch (figuras[i].QuemSou())
                        {
                            case "Entidade":
                                BoxEntidade.Visible = true;
                                NomeEntidade.Text = this.selecionado.GetName();
                                txtEntidadeX.Text = this.selecionado.GetX().ToString();
                                txtEntidadeY.Text = this.selecionado.GetY().ToString();

                                break;
                            case "Relacionamento":
                                BoxRelacionamento.Visible = true;
                                DonoRelacionamento2.Visible = false;
                                DonoRelacionamento3.Visible = false;
                                EntidadeDono2.Visible = false;
                                EntidadeDono3.Visible = false;

                                NomeRelacionamento.Text = this.selecionado.GetName();
                                RelacionamentoX.Text = this.selecionado.GetX().ToString();
                                RelacionamentoY.Text = this.selecionado.GetY().ToString();
                                Relacionamento rela = (Relacionamento)this.selecionado;
                                EntidadeDono1.Items.Clear();
                                EntidadeDono2.Items.Clear();
                                EntidadeDono3.Items.Clear();

                                foreach (Desenho entidades in figuras)
                                {
                                    if (entidades.QuemSou().Equals("Entidade"))
                                    {
                                        EntidadeDono1.Items.Add(entidades);
                                    }
                                }
                                foreach (Desenho entidades in figuras)
                                {
                                    if (entidades.QuemSou().Equals("Entidade"))
                                    {
                                        EntidadeDono2.Items.Add(entidades);
                                    }
                                }
                                foreach (Desenho entidades in figuras)
                                {
                                    if (entidades.QuemSou().Equals("Entidade"))
                                    {
                                        EntidadeDono3.Items.Add(entidades);
                                    }
                                }

                                List<Entidade> env = new List<Entidade>(rela.getEnvolvidos());
                                if (rela.getQtdEnvolvidos() > 0)
                                    EntidadeDono1.Text = env[0].GetName();
                                if (rela.getQtdEnvolvidos() > 1)
                                {
                                    EntidadeDono2.SelectedText = "";
                                    EntidadeDono2.SelectedText = env[1].GetName();
                                    DonoRelacionamento2.Visible = true;
                                    EntidadeDono2.Visible = true;
                                }
                                if (rela.getQtdEnvolvidos() > 2)
                                {
                                    EntidadeDono3.SelectedText = "";
                                    EntidadeDono3.SelectedText = env[2].GetName();
                                    DonoRelacionamento3.Visible = true;
                                    EntidadeDono3.Visible = true;
                                }
                                break;
                            case "Atributo":
                                BoxAtributo.Visible = true;
                                Atributo Aselecionado = (Atributo)figuras[i];
                                NomeAtributo.Text = Aselecionado.GetName();
                                cbDonoAtributo.Items.Clear();
                                int qual = 0;
                                foreach (Desenho g in figuras)
                                {
                                    if (g.QuemSou().Equals("Entidade") || g.QuemSou().Equals("Relacionamento"))
                                    {
                                        cbDonoAtributo.Items.Insert(qual, g);
                                        qual++;
                                        if (Aselecionado.GetProprietario() == g)
                                        {
                                            cbDonoAtributo.SelectedItem = g;
                                        }
                                    }
                                }
                                Dados dadoDoAtributo = Aselecionado.GetDados();
                                cbTipoAtributo.Items.Clear();
                                foreach (KeyValuePair<int, string> itemDado in dadoDoAtributo.GetDados())
                                {
                                    cbTipoAtributo.Items.Insert(itemDado.Key, itemDado.Value);
                                    if (itemDado.Value.Equals(Aselecionado.GetDados().GetDado()))
                                    {
                                        cbTipoAtributo.SelectedIndex = itemDado.Key;
                                    }
                                }
                                List<int> tipoA = Aselecionado.GetPropriedade();
                                if (tipoA[0] == 1)
                                    Primario.Checked = true;
                                else
                                    Primario.Checked = false;

                                if (tipoA[1] == 1)
                                    Composto.Checked = true;
                                else
                                    Composto.Checked = false;

                                if (tipoA[2] == 1)
                                {
                                    Derivado.Checked = true;
                                    cbDerivado.Visible = true;
                                    cbDerivado.Items.Clear();
                                    foreach (Desenho k in figuras)
                                    {
                                        if (k.QuemSou() == "Atributo")
                                        {
                                            cbDerivado.Items.Add(k);
                                        }
                                        if (Aselecionado.GetDerivado() == k)
                                        {
                                            cbDerivado.SelectedItem = k;
                                        }
                                    }
                                }
                                else
                                {
                                    Derivado.Checked = false;
                                    cbDerivado.Visible = false;
                                }

                                txtCardAtributoMin.Text = tipoA[3].ToString();

                                string str;
                                if (tipoA[4] == 2)
                                {
                                    str = "n";
                                }
                                else
                                {
                                    str = "1";
                                }

                                txtCardAtributoMax.Text = str;
                                break;
                        }
                    }
                }
            }

            if (desenhandoAtributo)
            {
                InfoOqueFazer.Text = "Selecione uma entidade, relacionamento ou atributo";
                for (int i = figuras.Count - 1; i >= 0; i--)
                {
                    string str = figuras[i].QuemSou();
                    if ((str.Equals("Entidade") || str.Equals("Relacionamento")) && figuras[i].GetArea(e.X, e.Y) == true)
                    {
                        value = "Atributo";
                        if (InputBox("Novo Atributo", "Nome Atributo:", ref value) == DialogResult.OK)
                        {
                            figuras.Add(new Atributo(value, figuras[i].GetX(), figuras[i].GetY() + 50, figuras[i]));
                            figuras.Last().SeDesenhe(GrpImage);
                            pn_edit.Refresh();
                            desenhandoAtributo = false;
                            this.textBox1.Focus();
                            break;
                        }
                    }
                    if ((str.Equals("Atributo")) && figuras[i].GetArea(e.X, e.Y) == true)
                    {
                        value = "Atributo Composto";
                        if (InputBox("Novo Atributo", "Nome Atributo:", ref value) == DialogResult.OK)
                        {
                            Atributo at = (Atributo)figuras[i];
                            int cordXAtributo = at.GetX();
                            int cordYAtributo = at.GetY();
                            if (at.GetTipo() == "Comum")
                            {
                                cordXAtributo += 18 + at.GetTam();
                                cordYAtributo += 28 + (at.GetIndice() * 14);
                            }
                            else if (at.GetTipo() == "Primario")
                            {
                                cordXAtributo += 18 + at.GetTam();
                                cordYAtributo += 12 - (at.GetIndice() * 14);
                            }

                            figuras.Add(new Atributo(value, cordXAtributo, cordYAtributo, figuras[i]));
                            figuras.Last().SeDesenhe(GrpImage);
                            pn_edit.Refresh();
                            desenhandoAtributo = false;
                            this.textBox1.Focus(); ;
                            break;
                        }
                    }
                }
                InfoOqueFazer.Text = "";
            }
            if (desenhandoPadronizacao)
            {
                InfoOqueFazer.Text = "Selecione as etidades padronizadas";
                click++;
                this.achou = false;
                if (click == 1)
                {
                    this.achou = false;
                    value = "Generalização";
                    string d = "2";
                    if (InputBox("Nova Generalização", "Nome Generalização:", ref value) == DialogResult.OK)
                    {
                        InputBox("Numero", "Quantidade de Entidades envolvidas:", ref d);
                        this.envolvidos = int.Parse(d);
                        figuras.Add(new Padronizacao(value, e.X, e.Y));
                    }
                    else
                    {
                        click--;
                    }
                }
                else if (click == 2)
                {
                    for (int i = 0; i < (figuras.Count) - 1; i++)
                    {
                        if (figuras[i].QuemSou() == "Entidade" && figuras[i].GetArea(e.X, e.Y) == true)
                        {
                            Padronizacao spec = (Padronizacao)figuras[figuras.Count - 1];
                            spec.SetPadrao((Entidade)figuras[i]);
                            achou = true;
                            break;
                        }
                    }
                    if (!achou)
                        click--;
                }
                else
                {
                    for (int i = 0; i < (figuras.Count) - 1; i++)
                    {
                        if (figuras[i].QuemSou() == "Entidade" && figuras[i].GetArea(e.X, e.Y) == true)
                        {
                            Padronizacao spec = (Padronizacao)figuras[figuras.Count - 1];
                            spec.AddEntidade((Entidade)figuras[i]);
                            envolvidos--;
                            break;
                        }
                    }
                    if (envolvidos == 0)
                    {
                        figuras.Last().SeDesenhe(GrpImage);
                        pn_edit.Refresh();
                        desenhandoPadronizacao = false;
                        this.textBox1.Focus(); ;
                        click = 0;
                        InfoOqueFazer.Text = "";
                    }
                }


            }
            if (desenhandoEspecializacao)
            {
                InfoOqueFazer.Text = "Selecione as etidades especializadas";
                click++;
                this.achou = false;
                if (click == 1)
                {
                    this.achou = false;
                    value = "Especialização";
                    string d = "2";
                    if (InputBox("Nova Especialização", "Nome Especialização:", ref value) == DialogResult.OK)
                    {
                        InputBox("Numero", "Quantidade de Entidades envolvidas:", ref d);
                        this.envolvidos = int.Parse(d);
                        figuras.Add(new Especializacao(value, e.X, e.Y));
                    }
                    else
                    {
                        click--;
                    }
                }
                else if (click == 2)
                {
                    for (int i = 0; i < (figuras.Count) - 1; i++)
                    {
                        if (figuras[i].QuemSou() == "Entidade" && figuras[i].GetArea(e.X, e.Y) == true)
                        {
                            Especializacao spec = (Especializacao)figuras[figuras.Count - 1];
                            spec.setEntidadeEspecializada((Entidade)figuras[i]);
                            achou = true;
                            break;
                        }
                    }
                    if (!achou)
                        click--;
                }
                else
                {
                    for (int i = 0; i < (figuras.Count) - 1; i++)
                    {
                        if (figuras[i].QuemSou() == "Entidade" && figuras[i].GetArea(e.X, e.Y) == true)
                        {
                            Especializacao spec = (Especializacao)figuras[figuras.Count - 1];
                            spec.addEntidades((Entidade)figuras[i]);
                            envolvidos--;
                            break;
                        }
                    }
                    if (envolvidos == 0)
                    {
                        figuras.Last().SeDesenhe(GrpImage);
                        pn_edit.Refresh();
                        desenhandoEspecializacao = false;
                        this.textBox1.Focus(); ;
                        click = 0;
                        InfoOqueFazer.Text = "";
                    }
                }


            }
            if (desenhandoEntidade)
            {
                value = "Entidade";
                if (InputBox("Nova Entidade", "Nome Entidade:", ref value) == DialogResult.OK)
                {
                    figuras.Add(new Entidade(value, e.X, e.Y));
                    figuras[figuras.Count - 1].SeDesenhe(GrpImage);
                    pn_edit.Refresh();
                }
                desenhandoEntidade = false;
                this.textBox1.Focus(); ;
            }
            if (desenhandoRelacionamento)
            {
                InfoOqueFazer.Text = "Selecione uma entidade";
                click++;
                this.achou = false;
                switch (click)
                {
                    case 1:
                        value = "Relacionamento";
                        string d = "2";
                        if (InputBox("Novo Relacionamento", "Nome Relacionamento:", ref value) == DialogResult.OK)
                        {
                            InputBox("Numero de 1 a 3", "Quantidade de Entidades envolvidas (1 a 3):", ref d);
                            figuras.Add(new Relacionamento(value, e.X, e.Y, Convert.ToInt16(d)));
                        }
                        else
                        {
                            click--;
                        }
                        break;
                    case 2:
                        for (int i = 0; i < (figuras.Count) - 1; i++)
                        {
                            if (figuras[i].QuemSou() == "Entidade" && figuras[i].GetArea(e.X, e.Y) == true)
                            {
                                Relacionamento r = (Relacionamento)figuras[figuras.Count - 1];
                                r.relacionarEntidade((Entidade)figuras[i]);
                                /// CRIA CARDINALIDADE
                                string cad = "1,1";
                                InputBox("Cardinalidade", "Cardinalidade", ref cad);
                                Cardinalidade c = new Cardinalidade(figuras[i].GetX(), figuras[i].GetY() + 55);
                                string[] t = cad.Split(',');
                                c.setCardMin(t[0]);
                                c.setCardMax(t[1]);
                                //////// FINALIZA
                                r.adicionarCardinalidade(c);
                                if (r.getQtdEnvolvidos() == 1)
                                {
                                    r.SeDesenhe(GrpImage);
                                    pn_edit.Refresh();
                                    desenhandoRelacionamento = false;
                                    this.textBox1.Focus(); ;
                                    click = 0;
                                    InfoOqueFazer.Text = "";
                                }
                                achou = true;
                                break;
                            }
                        }
                        if (!achou)
                            click--;
                        break;
                    case 3:
                        for (int i = 0; i < figuras.Count; i++)
                        {
                            if (figuras[i].QuemSou() == "Entidade" && figuras[i].GetArea(e.X, e.Y) == true)
                            {
                                Relacionamento r = (Relacionamento)figuras[figuras.Count - 1];
                                r.relacionarEntidade((Entidade)figuras[i]);
                                /// CRIA CARDINALIDADE
                                string cad = "1,1";
                                InputBox("Cardinalidade", "Cardinalidade", ref cad);
                                Cardinalidade c = new Cardinalidade(figuras[i].GetX(), figuras[i].GetY() + 55);
                                string[] t = cad.Split(',');
                                c.setCardMin(t[0]);
                                c.setCardMax(t[1]);
                                //////// FINALIZA
                                r.adicionarCardinalidade(c);
                                if (r.getQtdEnvolvidos() == 2)
                                {
                                    r.SeDesenhe(GrpImage);
                                    pn_edit.Refresh();
                                    desenhandoRelacionamento = false;
                                    this.textBox1.Focus(); ;
                                    click = 0;
                                    InfoOqueFazer.Text = "";
                                }
                                achou = true;
                                break;
                            }
                        }
                        if (!achou)
                            click--;
                        break;
                    case 4:
                        for (int i = 0; i < figuras.Count; i++)
                        {
                            if (figuras[i].QuemSou() == "Entidade" && figuras[i].GetArea(e.X, e.Y) == true)
                            {
                                Relacionamento r = (Relacionamento)figuras[figuras.Count - 1];
                                r.relacionarEntidade((Entidade)figuras[i]);
                                /// CRIA CARDINALIDADE
                                string cad = "1,1";
                                InputBox("Cardinalidade", "Cardinalidade", ref cad);
                                Cardinalidade c = new Cardinalidade(figuras[i].GetX(), figuras[i].GetY() + 55);
                                string[] t = cad.Split(',');
                                c.setCardMin(t[0]);
                                c.setCardMax(t[1]);
                                //////// FINALIZA
                                r.adicionarCardinalidade(c);
                                r.SeDesenhe(GrpImage);
                                pn_edit.Refresh();
                                desenhandoRelacionamento = false;
                                this.textBox1.Focus(); ;
                                click = 0;
                                InfoOqueFazer.Text = "";
                                achou = true;
                                break;
                            }
                        }
                        if (!achou)
                            click--;
                        break;
                }
            }
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();

            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancelar";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor |= AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;

        }
        public static DialogResult InputMessage(string title, string promptText)
        {
            Form form = new Form();


            Label label = new Label();
            Label textBox = new Label();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = "Salvo no banco de dados";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancelar";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor |= AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            return dialogResult;


        }
    }
}
