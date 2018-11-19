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

        public void moveUp()
        {
            input.CastKeyUp(input.ScanCodes.A);
            input.CastKeyUp(input.ScanCodes.S);
            input.CastKeyUp(input.ScanCodes.D);
            input.CastKeyDown(input.ScanCodes.W);
        }

        public void moveDown()
        {
            input.CastKeyUp(input.ScanCodes.W);
            input.CastKeyUp(input.ScanCodes.A);
            input.CastKeyUp(input.ScanCodes.D);
            input.CastKeyDown(input.ScanCodes.S);
        }

        public void moveLeft()
        {
            input.CastKeyUp(input.ScanCodes.W);
            input.CastKeyUp(input.ScanCodes.S);
            input.CastKeyUp(input.ScanCodes.D);
            input.CastKeyDown(input.ScanCodes.A);
        }

        public void moveRight()
        {
            input.CastKeyUp(input.ScanCodes.W);
            input.CastKeyUp(input.ScanCodes.A);
            input.CastKeyUp(input.ScanCodes.S);
            input.CastKeyDown(input.ScanCodes.D);
        }

        public void moveUpRight()
        {
            input.CastKeyUp(input.ScanCodes.A);
            input.CastKeyUp(input.ScanCodes.S);
            input.CastKeyDown(input.ScanCodes.W);
            input.CastKeyDown(input.ScanCodes.D);
        }

        public void moveUpLeft()
        {
            input.CastKeyUp(input.ScanCodes.A);
            input.CastKeyUp(input.ScanCodes.D);
            input.CastKeyDown(input.ScanCodes.W);
            input.CastKeyDown(input.ScanCodes.S);
        }

        public void moveDownRight()
        {
            input.CastKeyUp(input.ScanCodes.W);
            input.CastKeyUp(input.ScanCodes.A);
            input.CastKeyDown(input.ScanCodes.S);
            input.CastKeyDown(input.ScanCodes.D);
        }

        public void moveDownLeft()
        {
            input.CastKeyUp(input.ScanCodes.W);
            input.CastKeyUp(input.ScanCodes.D);
            input.CastKeyDown(input.ScanCodes.S);
            input.CastKeyDown(input.ScanCodes.A);
        }

        public void setFacingToPoint(PointF point)
        {
            double angle = angleToPoint(point)+270;
            if (angle > 359) angle -= 360;
            setFacing((float)angle);
        }

        public void moveToPoint(PointF point, float x)
        {
            while (getDistance(point) > x)
            {
                switch (moveDirection(point))
                {
                    case "down":
                        moveDown();
                        Thread.Sleep(100);
                        break;
                    case "downLeft":
                        moveDownLeft();
                        Thread.Sleep(100);
                        break;
                    case "left":
                        moveLeft();
                        Thread.Sleep(100);
                        break;
                    case "upLeft":
                        moveUpLeft();
                        Thread.Sleep(100);
                        break;
                    case "up":
                        moveUp();
                        Thread.Sleep(100);
                        break;
                    case "upRight":
                        moveUpRight();
                        Thread.Sleep(100);
                        break;
                    case "right":
                        moveRight();
                        Thread.Sleep(100);
                        break;
                    case "downRight":
                        moveDownRight();
                        Thread.Sleep(100);
                        break;
                }
            }
            input.CastKeyUp(input.ScanCodes.W);
            input.CastKeyUp(input.ScanCodes.A);
            input.CastKeyUp(input.ScanCodes.S);
            input.CastKeyUp(input.ScanCodes.D);
        }

        public double angleToPoint(PointF point)
        {
            PointF position = coordsToPoint();
            double angle = Math.Atan2((position.Y - point.Y), (position.X - point.X));
            return angle * 180 / Math.PI;
        }

        public String moveDirection(PointF point)
        {
            double angle = angleToPoint(point);
            if (angle <= 56 && angle > 15) return "down";
            if (angle <= 15 && angle > -18) return "downLeft";
            if (angle <= -18 && angle > -59) return "left";
            if (angle <= -59 && angle > -107) return "upLeft";
            if (angle <= -107 && angle > -155) return "up";
            if ((angle <= -155 && angle >= -180) || angle <= 180 && angle > 155) return "upRight";
            if (angle <= 155 && angle > 108) return "right";
            if (angle <= 108 && angle > 56) return "downRight";
            else return "nullAngle";
        }

        public double getDistance(PointF point)
        {
            double a = (double)(point.X - coordsToPoint().X);
            double b = (double)(point.Y - coordsToPoint().Y);

            return Math.Sqrt(a * a + b * b);
        }
    }
}
