using netDxf.Tables;
using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using myPoint;

namespace myGenerator
{
    public class VacuumTable_Generator : Generator, IGenerator
    {
        private VacuumTable_Parameters? parameters = new VacuumTable_Parameters();
        private string outputFile = "generated.dxf";
        private double point_x = 0.0;
        private double point_y = 0.0;

        public override void configure(object[] config)
        {
            try
            {
                if (config != null)
                {
                    if (config[0] != null)
                    {
                        this.parameters = (config[0] as VacuumTable_Parameters);
                    }
                    if (config[1] != null)
                    {
                        this.outputFile = (config[1] as string);
                    }
                }
            }
            catch
            {

            }
        }

        public override void generate()
        {
            base.generate();

            this.point_x = 0.0;
            this.point_y = 0.0;

            this.generate_baseOutline(point_x, point_y);

            this.generate_sideOutline(point_x, point_y);
            this.generate_sideOutline(point_x + this.parameters.vacuum_x_size - this.parameters.vacuum_holder_width , point_y);
            this.generate_slot(point_x, point_y);
            this.generate_slot(point_x + this.parameters.vacuum_x_size - this.parameters.vacuum_holder_width, point_y);

            this.generate_topOutline(point_x, point_y);
            this.generate_topInnerline(point_x, point_y);

            for (int x = 0; x < this.parameters.vacuum_square_x_cnt; x++)
            {
                for (int y = 0; y < this.parameters.vacuum_square_y_cnt; y++)
                {
                    double offset_x = (this.parameters.vacuum_square_x_size + this.parameters.vacuum_slot_width) * x;
                    double offset_y = (this.parameters.vacuum_square_y_size + this.parameters.vacuum_slot_width) * y;
                    offset_x += this.parameters.vacuum_holder_width + this.parameters.vacuum_wand_width + this.parameters.vacuum_slot_width;
                    offset_y += this.parameters.vacuum_wand_width + this.parameters.vacuum_slot_width;
                    this.generate_topSquare(point_x + offset_x, point_y + offset_y);
                }
            }
            doc.Save(this.outputFile);
        }

        private void generate_baseOutline(double point_x2, double point_y2) 
        {
            generate_square_with_rounded_edges(point_x2, point_y2, this.parameters.vacuum_x_size, this.parameters.vacuum_y_size, this.parameters.vacuum_fillet_radius);
        }

        private void generate_sideOutline(double point_x2, double point_y2) 
        {
            generate_square_with_rounded_edges(point_x2, point_y2, this.parameters.vacuum_holder_width, this.parameters.vacuum_y_size, this.parameters.vacuum_fillet_radius);
        }
        private void generate_slot(double point_x2, double point_y2) 
        {
            MyPoint start = new MyPoint(0.0, 0.0);
            MyPoint offset = new MyPoint(point_x2 + ((this.parameters.vacuum_holder_width - this.parameters.vacuum_holder_slot_width) / 2.0), point_y2 + (1.0 * this.parameters.vacuum_holder_width)); 
            double rotate_deg = 0.0;
            double rotate = (rotate_deg / 180.0) * Math.PI;
            double material_thickness = this.parameters.material_thickness;
            Boolean dog_bone_mode = this.parameters.dog_bone_mode;
            double milling_tool_diameter = this.parameters.milling_tool_diameter;
            DxfDocument? doc = this.doc;
            Layer? layerBlack = this.layerBlack;
            GeneratorLine gl = new GeneratorLine(start, offset, rotate, material_thickness, dog_bone_mode, milling_tool_diameter, doc, layerBlack);
            gl.AddCircle(new MyPoint((this.parameters.vacuum_holder_slot_width), (0.0))); 
            gl.Add(new MyPoint((this.parameters.vacuum_holder_slot_width), (this.parameters.vacuum_y_size - (2.0 * this.parameters.vacuum_holder_width))));
            gl.AddCircle(new MyPoint((0.0), (this.parameters.vacuum_y_size - (2.0 * this.parameters.vacuum_holder_width)))); 
            gl.Add(new MyPoint((0.0), (0.0)));
            gl.Finished();
        }

        private void generate_topOutline(double point_x2, double point_y2) 
        {
            double x_size = this.parameters.vacuum_x_size - (2.0 * this.parameters.vacuum_holder_width);
            double y_size = this.parameters.vacuum_y_size;
            generate_square_with_rounded_edges(point_x2 + this.parameters.vacuum_holder_width, point_y2, x_size, y_size, this.parameters.vacuum_fillet_radius);
        }
        private void generate_topInnerline(double point_x2, double point_y2) 
        {
            double x_size = this.parameters.vacuum_x_size - (2.0 * this.parameters.vacuum_holder_width) - (2.0 * this.parameters.vacuum_wand_width);
            double y_size = this.parameters.vacuum_y_size - (2.0 * this.parameters.vacuum_wand_width);
            generate_square_with_rounded_edges(point_x2 + this.parameters.vacuum_holder_width + this.parameters.vacuum_wand_width, point_y2 + this.parameters.vacuum_wand_width, x_size, y_size, this.parameters.vacuum_fillet_radius);
        }
        private void generate_topSquare(double point_x2, double point_y2) 
        {
            generate_square_with_rounded_edges(point_x2, point_y2, this.parameters.vacuum_square_x_size, this.parameters.vacuum_square_y_size, this.parameters.vacuum_fillet_radius);
        }


        private void generate_square(double point_x2, double point_y2, double x_len, double y_len)
        {
            generate_square_with_rounded_edges(point_x2, point_y2, x_len, y_len, 0.0);
        }
        private void generate_square_with_rounded_edges(double point_x2, double point_y2, double x_len, double y_len, double radius)
        {
            MyPoint start = new MyPoint(0.0, 0.0);
            MyPoint offset = new MyPoint(point_x2, point_y2);
            double rotate_deg = 0.0;
            double rotate = (rotate_deg / 180.0) * Math.PI;
            double material_thickness = this.parameters.material_thickness;
            Boolean dog_bone_mode = this.parameters.dog_bone_mode;
            double milling_tool_diameter = this.parameters.milling_tool_diameter;
            DxfDocument? doc = this.doc;
            Layer? layerBlack = this.layerBlack;
            GeneratorLine gl = new GeneratorLine(start, offset, rotate, material_thickness, dog_bone_mode, milling_tool_diameter, doc, layerBlack);
            gl.AddWithFillet(new MyPoint(x_len, (0.0)), radius);
            gl.AddWithFillet(new MyPoint(x_len, y_len), radius);
            gl.AddWithFillet(new MyPoint((0.0), y_len), radius);
            //gl.AddWithFillet(new MyPoint((0.0), (0.0)), radius);//Not needed because poly is closed

            if (radius != 0.0)
            {
                FilletHelper.AddFilletToPolyline2D(gl.poly, 3, radius);
                FilletHelper.AddFilletToPolyline2D(gl.poly, 2, radius);
                FilletHelper.AddFilletToPolyline2D(gl.poly, 1, radius);
                FilletHelper.AddFilletToPolyline2D(gl.poly, 0, radius);
            }

            gl.Finished();
        }
    }
}
