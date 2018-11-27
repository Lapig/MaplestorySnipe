using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Binarysharp.MemoryManagement;

namespace MaplestorySnipe
{
    class LocalPlayer
    {
        VAMemory vam = new VAMemory("maplestory2");
        static Process GameProcess = Process.GetProcessesByName("maplestory2").FirstOrDefault();
        
        public IntPtr localPlayerBase = GameProcess.MainModule.BaseAddress + 0x166BA64;
        public struct OffSets
        {
            internal const int MOVESPEED_1 = 0x1B4;
            internal const int MOVESPEED_2 = 0x400;
            internal const int MOUNTSPEED_1 = 0x1B4;
            internal const int MOUNTSPEED_2 = 0x490;
            internal const int FLY_MOUNTSPEED_1 = 0x1B4;
            internal const int FLY_MOUNTSPEED_2 = 0xE4;
            internal const int FLY_MOUNTSPEED_3 = 0x14;
            internal const int FLY_MOUNTSPEED_4 = 0xAC;
            internal const int ATKSPEED_1 = 0x1B4;
            internal const int ATKSPEED_2 = 0x3F8;
            internal const int JUMPHEIGHT_1 = 0x1B4;
            internal const int JUMPHEIGHT_2 = 0x440;
            internal const int Z_COORD_1 = 0x1B4;
            internal const int Z_COORD_2 = 0xE8;
            internal const int Z_COORD_3 = 0x4;
            internal const int Z_COORD_4 = 0x11C;
            internal const int X_COORD_1 = 0x1B4;
            internal const int X_COORD_2 = 0xE8;
            internal const int X_COORD_3 = 0x4;
            internal const int X_COORD_4 = 0x10C;
            internal const int Y_COORD_1 = 0x1B4;
            internal const int Y_COORD_2 = 0xE8;
            internal const int Y_COORD_3 = 0x4;
            internal const int Y_COORD_4 = 0x114;
            internal const int FACE_ANGLE_1 = 0x1B4;
            internal const int FACE_ANGLE_2 = 0x1438;
            internal const int DELTA_SPEED_1 = 0x1B4;
            internal const int DELTA_SPEED_2 = 0x120;
            internal const int JUMP_DISTANCE_1 = 0x1B4;
            internal const int JUMP_DISTANCE_2 = 0x134;
            internal const int SIZE_1 = 0x1B4;
            internal const int SIZE_2 = 0x80;
        }
        public struct PlayerCoordinates
        {
            internal float Z_AXIS;
            internal float X_AXIS;
            internal float Y_AXIS;
        }

        public IntPtr getAddressLevelTwo(IntPtr baseAddress, int one, int two)
        {
            IntPtr Base = baseAddress;
            IntPtr base1 = IntPtr.Add((IntPtr)vam.ReadInt32(Base), one);
            IntPtr base2 = IntPtr.Add((IntPtr)vam.ReadInt32(base1), two);
            return base2;
        }

        public IntPtr getAddressLevelFour(IntPtr baseAddress, int one, int two, int three, int four)
        {
            IntPtr Base = baseAddress;
            IntPtr base1 = IntPtr.Add((IntPtr)vam.ReadInt32(Base), one);
            IntPtr base2 = IntPtr.Add((IntPtr)vam.ReadInt32(base1), two);
            IntPtr base3 = IntPtr.Add((IntPtr)vam.ReadInt32(base2), three);
            IntPtr base4 = IntPtr.Add((IntPtr)vam.ReadInt32(base3), four);
            return base4;
        }

        public void writeValue(IntPtr address, int x)
        {
            vam.WriteInt32(address, x);
        }

        public void writeValueFloat(IntPtr address, float x)
        {
            vam.WriteFloat(address, x);
        }

        public void writeValueDouble(IntPtr address, double x)
        {
            vam.WriteDouble(address, x);
        }

        public int getValue(IntPtr address)
        {
            return vam.ReadInt32(address);
        }

        public float getValueFloat(IntPtr address)
        {
            return vam.ReadFloat(address);
        }

        public int AttackSpeed()
        {
            return getValue(getAddressLevelTwo(localPlayerBase, OffSets.ATKSPEED_1, OffSets.ATKSPEED_2));
        }

        public int MovementSpeed()
        {
            return getValue(getAddressLevelTwo(localPlayerBase, OffSets.MOVESPEED_1, OffSets.MOVESPEED_2));
        }

        public float FlyingMountSpeed()
        {
            return getValueFloat(getAddressLevelFour(localPlayerBase, OffSets.FLY_MOUNTSPEED_1, OffSets.FLY_MOUNTSPEED_2, OffSets.FLY_MOUNTSPEED_3, OffSets.FLY_MOUNTSPEED_4));
        }

        public int MountSpeed()
        {
            return getValue(getAddressLevelTwo(localPlayerBase, OffSets.MOUNTSPEED_1, OffSets.MOUNTSPEED_2));
        }

        public int JumpHeight()
        {
            return getValue(getAddressLevelTwo(localPlayerBase, OffSets.JUMPHEIGHT_1, OffSets.JUMPHEIGHT_2));
        }

        public float Facing()
        {
            return getValueFloat(getAddressLevelTwo(localPlayerBase, OffSets.FACE_ANGLE_1, OffSets.FACE_ANGLE_2));
        }

        public float CharSize()
        {
            return getValueFloat(getAddressLevelTwo(localPlayerBase, OffSets.SIZE_1, OffSets.SIZE_2));
        }

        public float DeltaSpeed()
        {
            return getValueFloat(getAddressLevelTwo(localPlayerBase, OffSets.DELTA_SPEED_1, OffSets.DELTA_SPEED_2));
        }

        public PlayerCoordinates Coords()
        {
            return getCoords();
        }

        public void setAttackSpeed(int x)
        {
            writeValue((getAddressLevelTwo(localPlayerBase, OffSets.ATKSPEED_1, OffSets.ATKSPEED_2)), x);
        }

        public void setMovementSpeed(int x)
        {
            writeValue((getAddressLevelTwo(localPlayerBase, OffSets.MOVESPEED_1, OffSets.MOVESPEED_2)), x);
        }

        public void setFlyingMountSpeed(float x)
        {
            writeValueFloat((getAddressLevelFour(localPlayerBase, OffSets.FLY_MOUNTSPEED_1, OffSets.FLY_MOUNTSPEED_2, OffSets.FLY_MOUNTSPEED_3, OffSets.FLY_MOUNTSPEED_4)), x);
        }

        public void setMountSpeed(int x)
        {
            writeValue((getAddressLevelTwo(localPlayerBase, OffSets.MOUNTSPEED_1, OffSets.MOUNTSPEED_2)), x);
        }

        public void setJumpHeight(int x)
        {
            writeValue((getAddressLevelTwo(localPlayerBase, OffSets.JUMPHEIGHT_1, OffSets.JUMPHEIGHT_2)), x);
        }

        public void setFacing(float x)
        {
            writeValueFloat(getAddressLevelTwo(localPlayerBase, OffSets.FACE_ANGLE_1, OffSets.FACE_ANGLE_2), x);
        }

        public void setCharSize(float x)
        {
            writeValueFloat(getAddressLevelTwo(localPlayerBase, OffSets.SIZE_1, OffSets.SIZE_2), x);
        }

        public void setDeltaSpeed(float x)
        {
            writeValueFloat(getAddressLevelTwo(localPlayerBase, OffSets.DELTA_SPEED_1, OffSets.DELTA_SPEED_2), x);
        }

        private PlayerCoordinates getCoords()
        {
            PlayerCoordinates coordinates = new PlayerCoordinates
            {
                Z_AXIS = getValueFloat(getAddressLevelFour(localPlayerBase, OffSets.Z_COORD_1, OffSets.Z_COORD_2, OffSets.Z_COORD_3, OffSets.Z_COORD_4)),
                X_AXIS = getValueFloat(getAddressLevelFour(localPlayerBase, OffSets.X_COORD_1, OffSets.X_COORD_2, OffSets.X_COORD_3, OffSets.X_COORD_4)),
                Y_AXIS = getValueFloat(getAddressLevelFour(localPlayerBase, OffSets.Y_COORD_1, OffSets.Y_COORD_2, OffSets.Y_COORD_3, OffSets.Y_COORD_4))
            };
            return coordinates;
        }

        public PointF coordsToPoint()
        {
            return new PointF(getCoords().X_AXIS, getCoords().Y_AXIS);
        }

        public void teleport(float x, float y, float z)
        {
            writeValueFloat((getAddressLevelFour(localPlayerBase, OffSets.Z_COORD_1, OffSets.Z_COORD_2, OffSets.Z_COORD_3, OffSets.Z_COORD_4)), z);
            writeValueFloat((getAddressLevelFour(localPlayerBase, OffSets.X_COORD_1, OffSets.X_COORD_2, OffSets.X_COORD_3, OffSets.X_COORD_4)), x);
            writeValueFloat((getAddressLevelFour(localPlayerBase, OffSets.Y_COORD_1, OffSets.Y_COORD_2, OffSets.Y_COORD_3, OffSets.Y_COORD_4)), y);
        }
    }
}
