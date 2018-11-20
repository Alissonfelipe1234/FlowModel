﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowModel
{
    class Atributo:Desenho
    {
        private string nome;
        private int x;
        private int y;
        private int indice;
        private int qtdAtributos;
        private Dados dado;
        private Tipo propriedades;
        private Desenho proprietario;

        private int id;

        public Atributo(string n, int Px, int Py, Desenho p)
        {
            this.nome = n;
            this.x = Px;
            this.y = Py;
            this.dado = new Dados(1);
            this.propriedades = new Tipo();
            this.proprietario = p;
            this.qtdAtributos = 0;

            switch (p.QuemSou())
            {
                case "Entidade":
                    Entidade ent = (Entidade)this.proprietario;
                    this.indice = ent.getQtdAtributos();
                    ent.addAtributo();
                    break;
                case "Relacionamento":
                    Relacionamento rel = (Relacionamento)this.proprietario;
                    this.indice = rel.getQtdAtributos();
                    rel.addAtributo();
                    break;
                case "Atributo":
                    Atributo atr = (Atributo)this.proprietario;
                    this.indice = atr.getQtdAtributos();
                    atr.addAtributo();
                    this.propriedades.Altera(false, true, false, 1, 1);
                    break;
            }

        }

        public void addAtributo()
        {
            this.qtdAtributos++;
        }

        public int getQtdAtributos()
        {
            return this.qtdAtributos;
        }

        public void setDado(int idDado)
        {
            this.dado = new Dados(idDado);
        }
         
        public void AlteraTipo (List<int> status)
        {
            this.propriedades.Altera(Convert.ToBoolean(status[0]), Convert.ToBoolean(status[1]), Convert.ToBoolean(status[2]), status[3], status[4]);
        }

        public int getIndice()
        {
            return indice;
        }

        public string getName()
        {
            return nome;
        }

        public int getX()
        {
            return this.x;
        }

        public int getY()
        {
            return this.y;
        }

        public string getIdDado()
        {
            return this.dado.getDado();
        }

        public List<int> getPropriedade()
        {
            return this.propriedades.GetStatus();
        }
        public string getTipo()
        {
            return this.propriedades.getPropriedades();
        }
        public void comumToPrimario()
        {
            if(this.propriedades.getPropriedades().Equals("Comum"))
            {
                List<int> atual = this.propriedades.GetStatus();
                this.propriedades.Altera(true, Convert.ToBoolean(atual[1]), Convert.ToBoolean(atual[2]), atual[3], atual[4]);
                this.y = this.y - 52;
            }
        }
        public void comumToOpcional()
        {
            if (this.propriedades.getPropriedades().Equals("Comum"))
            {
                List<int> atual = this.propriedades.GetStatus();
                this.propriedades.Altera(Convert.ToBoolean(atual[0]), Convert.ToBoolean(atual[1]), Convert.ToBoolean(atual[2]), 0, atual[4]);
                this.x = this.x + 100;
                this.y = this.y - 52;
            }
        }
        public void PrimarioToComum()
        {
            if (this.propriedades.getPropriedades().Equals("Primario"))
            {
                List<int> atual = this.propriedades.GetStatus();
                this.propriedades.Altera(false, Convert.ToBoolean(atual[1]), Convert.ToBoolean(atual[2]), atual[3], atual[4]);
                this.y = this.y + 52;
            }
        }
        public void OpcionalToComum()
        {
            if (this.propriedades.getPropriedades().Equals("Opcional"))
            {
                List<int> atual = this.propriedades.GetStatus();
                this.propriedades.Altera(Convert.ToBoolean(atual[0]), Convert.ToBoolean(atual[1]), Convert.ToBoolean(atual[2]), 1, atual[4]);
                this.x = this.x - 100;
                this.y = this.y + 52;
            }
        }
        public string getSql()
        {
            string str = "";
            str += this.getName() + " ";
            str += this.dado.getDado() + " ";
            switch (this.propriedades.getPropriedades())
            {
                case "Primario":
                    str += "PRIMARY KEY";
                    break;
                case "Opcional":
                    break;
                case "Comum":
                    str += "NOT NULL";
                    break;
            }
            return str;
        }
        public Desenho getProprietario()
        {
            return this.proprietario;
        }
        public string QuemSou()
        {
            return "Atributo";
        }

        public void SeDesenhe(Graphics g, Panel p)
        {
            Image newImage;
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            switch (this.propriedades.getPropriedades())
            {                
                case "Primario":
                    newImage = Image.FromFile("C:\\Users\\aliss\\Desktop\\C#\\FlowModel\\FlowModel\\resources\\Atributo_Chave.png");
                    g.DrawImage(newImage, this.x, this.y);                    
                    g.DrawString(this.nome, new Font(new FontFamily("Arial"), 9), drawBrush, this.x + 18, this.y + 12 - (this.indice * 14));
                    break;
                case "Opcional":
                    newImage = Image.FromFile("C:\\Users\\aliss\\Desktop\\C#\\FlowModel\\FlowModel\\resources\\Atributo_Opcional.png");
                    g.DrawImage(newImage, this.x, this.y);
                    g.DrawString(this.nome, new Font(new FontFamily("Arial"), 9), drawBrush, this.x + 18, this.y + 12 - (this.indice * 14));
                    break;
                case "Composto":
                    newImage = Image.FromFile("C:\\Users\\aliss\\Desktop\\C#\\FlowModel\\FlowModel\\resources\\Atributo_Composto.png");
                    g.DrawImage(newImage, this.x, this.y);
                    g.DrawString(this.nome, new Font(new FontFamily("Arial"), 9), drawBrush, this.x + 40 + (this.indice * 50), this.y + 5);
                    break;
                case "Comum":
                    newImage = Image.FromFile("C:\\Users\\aliss\\Desktop\\C#\\FlowModel\\FlowModel\\resources\\Atributo_Simples.png");
                    g.DrawImage(newImage, this.x, this.y);
                    g.DrawString(this.nome, new Font(new FontFamily("Arial"), 9), drawBrush, this.x + 18, this.y + 28 +(this.indice * 14));
                    break;
            }
            this.proprietario.SeDesenhe(g,p);
            p.Refresh();
        }

        public void Propriedades(Panel p)
        {
            throw new NotImplementedException();
        }
        public int getTam()
        {
            Bitmap bmpImagem = new Bitmap(713, 599);
            Graphics medidor = Graphics.FromImage(bmpImagem);
            SizeF tamanhoString = medidor.MeasureString(this.nome, new Font(new FontFamily("Arial"), 9));
            return Convert.ToInt16(tamanhoString.Width);
        }

        public bool GetArea(int x, int y)
        {
            Bitmap bmpImagem = new Bitmap(713, 599);
            Graphics medidor = Graphics.FromImage(bmpImagem);
            SizeF tamanhoString = medidor.MeasureString(this.nome, new Font(new FontFamily("Arial"), 9));


            switch (this.propriedades.getPropriedades())
            {
                case "Comum":
                    if (x - this.x >= 18 && x - this.x <= tamanhoString.Width + 18)
                        if (y - this.y >= 28 + (this.indice * 12) && y - this.y <= 28 + (this.indice * 14) + tamanhoString.Height)
                            return true;
                    break;
                case "Primario":
                    if (x - this.x >= 18 && x - this.x <= tamanhoString.Width + 18)
                        if (y - this.y <= 12 - (this.indice * 12) && y - this.y >= 12 - (this.indice * 14)  + tamanhoString.Height)
                            return true;
                    break;
                case "Opcional":
                    if (x - this.x >= 18 && x - this.x <= tamanhoString.Width + 18)
                        if (y - this.y <= 12 - (this.indice * 12) && y - this.y >= 12 - (this.indice * 14) + tamanhoString.Height)
                            return true;
                    break;
                case "Composto":
                    if (x - this.x >= 18 && x - this.x <= tamanhoString.Width + 18)
                        if (y - this.y >= 28 + (this.indice * 12) && y - this.y <= 28 + (this.indice * 14) + tamanhoString.Height)
                            return true;
                    break;
            }


            return false;
        }

        
    }
}
