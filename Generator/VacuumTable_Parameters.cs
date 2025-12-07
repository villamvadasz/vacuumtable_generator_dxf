using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myGenerator
{
    public class VacuumTable_Parameters
    {
        public VacuumTable_Parameters()
        {
            this.vacuum_square_x_size = 25.0;
            this.vacuum_square_y_size = 25.0;
            this.vacuum_square_abrundung = 2.0;
            this.vacuum_square_x_cnt = 7;
            this.vacuum_square_y_cnt = 5;

            this.vacuum_slot_depth = 2.0;
            this.vacuum_slot_width = 2.7;

            this.vacuum_z_size = 20.0;
            this.vacuum_fase = 1.0;

            this.vacuum_holder_width = 12.4;
            this.vacuum_holder_slot_width = 6.2;

            this.vacuum_wand_width = 5.0;

            this.material_thickness = 20.0;
            this.dog_bone_mode = false;
            this.milling_tool_diameter = 6.0;

            this.vacuum_fillet_radius = 3.0;


    }

    public double vacuum_square_x_size { get; set; }
        public double vacuum_square_y_size { get; set; }
        public double vacuum_square_abrundung { get; set; }
        public int vacuum_square_x_cnt { get; set; }
        public int vacuum_square_y_cnt { get; set; }

        public double vacuum_slot_depth { get; set; }
        public double vacuum_slot_width { get; set; }

        internal double vacuum_x_size { get { return (vacuum_square_x_size + vacuum_slot_width) * vacuum_square_x_cnt + (2 * vacuum_holder_width) +(2 * vacuum_wand_width) + vacuum_slot_width; } }
        internal double vacuum_y_size { get { return (vacuum_square_y_size + vacuum_slot_width) * vacuum_square_y_cnt + (2 * vacuum_wand_width) + vacuum_slot_width; } }
        public double vacuum_z_size { get; set; }

        public double vacuum_fase { get; set; }
        public double vacuum_fillet_radius { get; set; }

        public double vacuum_holder_width { get; set; }
        public double vacuum_holder_slot_width { get; set; }

        public double vacuum_wand_width { get; set; }


        public double material_thickness { get; set; }
        public Boolean dog_bone_mode { get; set; }
        public double milling_tool_diameter { get; set; }

        internal double shorter_length { get { if (vacuum_x_size < vacuum_y_size) { return vacuum_x_size; } else { return vacuum_y_size; } } }
        internal double box_tounge_size { get { return shorter_length / 3.0; } }
        internal double box_tounge_min_space_length { get { return shorter_length / 3.0; } }
        internal double box_tounge_optimal_space_length { get { return shorter_length / 3.0; } }
        internal double tongue_hole_oversize_longitudional { get { return this.milling_tool_diameter / 2.0; } }
        internal double tongue_hole_oversize_horizontal { get { return 0.1; } }

    }

}
