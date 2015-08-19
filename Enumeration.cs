using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceGame
{
    public abstract class Enumeration
    {

        public enum LevelName
        {
            dungeon_prison = 1,
            dungeon_cave = 2,
            dungeon_guards = 3,
            dungeon_skeleton = 4,
            palace_mirror = 5,
            palace_thief = 6,
            palace_plunge = 7,
            dungeon_weightless = 8,
            dungeon_mouse = 9,
            dungeon_twisty = 10,
            palace_quad = 11,
            palace_fragile = 12,
            dungeon_tower = 13,
            dungeon_jaffar = 14,
            palace_rescue = 15,
            dungeon_potions = 16

        }

        public enum SpriteType
        {
            nothing,
            kid,
            sultan,
            guard,
            skeleton,
            serpent
        }

        public enum Input
        {
            left,
            right,
            down,
            up,
            leftshift,
            rightshift,
            shift,
            leftup,
            rightup,
            leftdown,
            righdown,
            none
        }

        public enum PriorityState
        {
            Normal,
            Force
        }

        public enum SequenceReverse
        {
            Normal,
            Reverse,
            //reverse all and reset frame to 0
            FixFrame
            //don't reset frame counter
        }

        public enum TileCollision
        {
            /// <summary>
            /// A passable tile is one which does not hinder player motion at all, like a space
            /// </summary>
            Passable = 0,

            /// <summary>
            /// An impassable tile is one which does not allow the player to move through
            /// it at all. It is completely solid, like a wall block
            /// </summary>
            Impassable = 1,

            /// <summary>
            /// Standard floor
            /// </summary>
            Platform = 2
        }

        public enum TileType
        {
            space = 0,
            floor = 1,
            spikes = 2,
            posts = 3,
            gate = 4,
            dpressplate = 5,
            //;down
            pressplate = 6,
            // ;up
            panelwif = 7,
            // ;w/floor  //AMF when loose shake?!?!!?!?
            pillarbottom = 8,
            pillartop = 9,
            flask = 10,
            loose = 11,
            panelwof = 12,
            // ;w/o floor
            mirror = 13,
            rubble = 14,
            upressplate = 15,
            exit = 16,
            //left door portion
            exit2 = 17,
            //right door portion
            slicer = 18,
            torch = 19,
            block = 20,
            bones = 21,
            sword = 22,
            window = 23,
            window2 = 24,
            archbot = 25,
            archtop1 = 26,
            archtop2 = 27,
            archtop3 = 28,
            archtop4 = 29,
            tile_torch_rubble = 30,
            //taked from apoplexy norbert
            tile_loose = 43,
            //taked from apoplexy norbert
            //door = 100 //changed from apoplexy norbert
            lava = 32,
            chomper = 33
        }

        //first 4 byte are element type the rest is the modifier, _x tells the result formula
        public enum Tile
        {
            space_1 = 0x0,
            space_2 = 0x1,
            space_3 = 0x2,
            space_4 = 0x3,
            space_5 = 0xff,

            floor_6 = 0x10000,
            floor_7 = 0x10001,
            floor_8 = 0x10002,
            floor_9 = 0x10003,
            floor_10 = 0x100ff,

            spike_11 = 0x20000,
            spike_12 = 0x20001,
            spike_13 = 0x20002,
            spike_14 = 0x20003,
            spike_15 = 0x20004,
            spike_16 = 0x20005,
            spike_17 = 0x20006,
            spike_18 = 0x20007,
            spike_19 = 0x20008,
            spike_20 = 0x20009,

            posts_21 = 0x30000,

            gate_22 = 0x40000,
            gate_23 = 0x40001,

            dpressplate_24 = 0x50000,

            pressplate_25 = 0x60000,

            panelwif_26 = 0x70000,
            panelwif_27 = 0x70001,
            panelwif_28 = 0x70002,
            panelwif_29 = 0x70003,

            pillarbottom_30 = 0x80000,

            pillartop_31 = 0x90001,

            flask_32 = 0x100000,
            flask_33 = 0x100001,
            flask_34 = 0x100002,
            flask_35 = 0x100003,
            flask_36 = 0x100004,
            flask_37 = 0x100005,
            flask_38 = 0x100006,

            loose_39 = 0x110000,

            panelwof_40 = 0x120000,
            panelwof_41 = 0x120001,
            panelwof_42 = 0x120002,
            panelwof_43 = 0x120003,
            panelwof_44 = 0x120004,
            panelwof_45 = 0x120005,
            panelwof_46 = 0x120006,
            panelwof_47 = 0x120007,

            mirror_48 = 0x130000,

            rubble_49 = 0x140000,

            upressplate_50 = 0x150000,

            exit_51 = 0x160000,
            exit2_52 = 0x170000,

            slicer_53 = 0x180000,
            slicer_54 = 0x180001,
            slicer_55 = 0x180002,
            slicer_56 = 0x180003,
            slicer_57 = 0x180004,
            slicer_58 = 0x180005,

            torch_59 = 0x190000,

            block_60 = 0x200000,
            block_61 = 0x200001,

            bones_62 = 0x210000,

            sword_63 = 0x220000,

            window_64 = 0x230000,
            window2_65 = 0x240000,

            archbot_66 = 0x250000,
            archtop1_67 = 0x260000,
            archtop2_68 = 0x270000,
            archtop3_69 = 0x280000,
            archtop4_70 = 0x290000,

            tile_torch_rubble_71 = 0x300000,
            tile_loose_72 = 0x430000

            //Dog = 0x00010000,
            //Cat = 0x00020000,
            //Alsation = Dog | 0x00000001,
            //Greyhound = Dog | 0x00000002,
            //Siamese = Cat | 0x00000001
        }

        public enum Modifier
        {
            none = 0,
            one = 1,
            two = 2,
            three = 3,
            four = 4,
            five = 5,
            six = 6,
            seven = 7,
            eight = 8,
            nine = 9,
            all = 255
        }

        public enum Items
        {
            nothing,
            sword,
            flask,
            flaskbig,
            potion
        }

        public enum StateTile
        {
            normal,
            // normal state
            close,
            //close animation
            closefast,
            //close fast animation
            closed,
            //close
            open,
            //opening animation
            opened,
            dpressplate,
            //;down
            pressplate,
            // ;up
            loose,
            //loose floor
            loosefall,
            //when loose fall
            rubble,
            //when floor break
            looseshake,
            //when loose shake but dont fall
            brick,
            //floor/space with brickwall
            mask,
            //mask state
            //,sword //sword tile now item!!
            bones,
            //bones

            kill,

            exit_close_left,
            exit_close_left_up,
            exit_close_right,
            exit_close_right_up,

            exit,

            exit2
        }



        public enum State
        {
            none,
            // my state
            question,
            //my state for interpretation
            crouch,
            //my state
            godown,
            //my state invert standup
            ready,
            //my state guard ready 
            startrun,
            stand,
            standjump,
            runjump,
            turn,
            runturn,
            stepfall,
            jumphangMed,
            hang,
            climbup,
            hangdrop,
            freefall,
            runstop,
            jumpup,
            fallhang,
            jumpbackhang,
            softland,
            jumpfall,
            stepfall2,
            medland,
            rjumpfall,
            hardland,
            hangfall,
            jumphangLong,
            hangstraight,
            rdiveroll,
            sdiveroll,
            highjump,
            step1,
            step2,
            step3,
            step4,
            step5,
            step6,
            step7,
            step8,
            step9,
            step10,
            step11,
            step12,
            step13,
            fullstep,
            turnrun,
            testfoot,
            bumpfall,
            hardbump,
            bump,
            superhijump,
            standup,
            stoop,
            impale,
            crush,
            deadfall,
            halve,
            //COMBAT ACTION
            engarde,
            advance,
            retreat,
            strike,
            flee,
            turnengarde,
            strikeblock,
            readyblock,
            landengarde,
            bumpengfwd,
            bumpengback,
            blocktostrike,
            strikeadv,
            climbdown,
            blockedstrike,
            climbstairs,
            dropdead,
            stepback,
            climbfail,
            stabbed,
            faststrike,
            strikeret,
            alertstand,
            drinkpotion,
            crawl,
            alertturn,
            fightfall,
            efightfall,
            efightfallfwd,
            running,
            stabkill,
            fastadvance,
            goalertstand,
            arise,
            turndraw,
            guardengarde,
            pickupsword,
            resheathe,
            fastsheathe,
            Pstand,
            Vstand,
            Vwalk,
            Vstop,
            Palert,
            Pback,
            Vexit,
            Mclimb,
            Vraise,
            Plie,
            patchfall,
            Mscurry,
            Mstop,
            Mleave,
            Pembrace,
            Pwaiting,
            Pstroke,
            Prise,
            Pcrouch,
            Pslump,
            Mraise,
            splash_player,
            // my state PoP.net 
            splash_enemy,
            // my state PoP.net
            delete,

            drinkpotionbig,

            burned
            //my state for delete and disrupt a object from memory
        }

        public enum TypeFrame
        {
            SPRITE,
            COMMAND
        }
        public enum TypeCommand
        {
            GOTOFRAME,
            GOTOSEQUENCE,
            ABOUTFACE,
            IFGOTOSEQUENCE,
            IFGOTOFRAME,
            DELETE
            //delete the sprite.. future implementation..??!?
        }

        public enum SpriteEffects
        {
            None = 0,
            FlipHorizontally = 1,
            FlipVertically = 2
        }


    }

}
