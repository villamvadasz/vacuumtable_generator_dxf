using netDxf.Tables;
using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf.Entities;

namespace myGenerator
{
    public class Generator : IGenerator
    {
        public DxfDocument? doc = null;
        public Layer? layerRed = null;
        public Layer? layerBlack = null;
        public Layer? layerHoles = null;
        public Layer? layerPCB = null;

        internal double part_inter_spacing = 30.0;//mm
        
        public virtual void configure(object[] config)
        {
            throw new NotImplementedException();
        }

        public virtual void generate()
        {
            this.doc = new DxfDocument();
            {
                this.layerRed = new Layer("RedLayer");
                this.layerRed.Color = AciColor.Red;
                doc.Layers.Add(this.layerRed);
            }
            {
                this.layerBlack = new Layer("BlackLayer");
                this.layerBlack.Color = AciColor.Black;
                doc.Layers.Add(this.layerBlack);
            }
            {
                this.layerHoles = new Layer("HolesLayer");
                this.layerHoles.Color = AciColor.Blue;
                doc.Layers.Add(this.layerHoles);
            }

            {
                this.layerPCB = new Layer("PCBLayer");
                this.layerPCB.Color = AciColor.Cyan;
                doc.Layers.Add(this.layerPCB);
            }
        }
        public void SaveDoc(string outputFile)
        {
            if (doc != null)
            {
                doc.Save(outputFile);
            }
        }

    }
}
