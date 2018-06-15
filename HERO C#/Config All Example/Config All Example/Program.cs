using Microsoft.SPOT;
using System.Threading;

using CTRE.Phoenix;
using CTRE.Phoenix.MotorControl.CAN;
using CTRE.Phoenix.Controller;
using CTRE.Phoenix.Sensors;
using System;

namespace HERO_Continuous_Position_Servo_Example1
{
    public class Program
    {
        static RobotApplication _robotApp = new RobotApplication();

        public static void Main()
        {
            _robotApp.init();

            while (true)
            {
                _robotApp.run();

                Thread.Sleep(10);
            }
        }
    }

    public class RobotApplication
    {
        /** make a talon with deviceId 0 */
        TalonSRX  _talon = new TalonSRX(1);
        VictorSPX  _victor = new VictorSPX(9);
        //PigeonIMU  _pigeon = new PigeonIMU(3);
        CANifier  _canifier = new CANifier(4);

        /** Use a USB gamepad plugged into the HERO */
        GameController _gamepad = new GameController(UsbHostDevice.GetInstance());

        /** hold the current button values from gamepad*/
        bool[] _btns = new bool[10];

        configs _custom_configs = new configs();

        /** hold the last button values from gamepad, this makes detecting on-press events trivial */
        bool[] _btnsLast = new bool[10];
        long _time_ran = (DateTime.UtcNow).Ticks;

        
        public void init()
        {
            Debug.Print("init");
            _btns[5] = true;
        }

        public void run()
        {
            Loop10Ms();
        }

        void Loop10Ms()
        {
            _btns[3] = (((DateTime.UtcNow).Ticks - _time_ran) / TimeSpan.TicksPerSecond > 5) || _btns[0];
            /* get all the buttons */
            //FillBtns(ref _btns);

            /*Debug.Print("buttons: " + _btns[0].ToString() + "," + _btns[1].ToString() 
                + "," + _btns[2].ToString() + "," + _btns[3].ToString() + "," 
                + _btns[4].ToString() + "," + _btns[5].ToString());
            Debug.Print("buttons last: " + _btnsLast[0].ToString() + "," + _btnsLast[1].ToString()
                + "," + _btnsLast[2].ToString() + "," + _btnsLast[3].ToString() + ","
                + _btnsLast[4].ToString() + "," + _btnsLast[5].ToString());*/
            if (_btns[0] && !_btnsLast[0])
            {
                Debug.Print("read talon");

                TalonSRXConfiguration read_talon;
                _talon.GetAllConfigs(out read_talon);

                Debug.Print(read_talon.ToString("_talon"));

            }
            else if (_btns[1] && !_btnsLast[1])
            {
                Debug.Print("read victor");

                VictorSPXConfiguration read_victor;
                _victor.GetAllConfigs(out read_victor);

                Debug.Print(read_victor.ToString("_victor"));

            }
            /*else if (_btns[2] && !_btnsLast[2])
            {

                Debug.Print("read pigeon");

                PigeonIMUConfiguration read_pigeon;
                _pigeon.GetAllConfigs(out read_pigeon);

                Debug.Print(read_pigeon.ToString("_pigeon"));

            }*/
            else if (_btns[3] && !_btnsLast[3])
            {
                Debug.Print("read canifier");

                CANifierConfiguration read_canifier;
                _canifier.GetAllConfigs(out read_canifier);

                Debug.Print(read_canifier.ToString("_canifier"));

            }
            else if (_btns[4] && !_btnsLast[4])
            {
                Debug.Print("custom config start");

                _talon.ConfigAllSettings(_custom_configs._talon);
                _victor.ConfigAllSettings(_custom_configs._victor);
                //_pigeon.ConfigAllSettings(_custom_configs._pigeon);
                _canifier.ConfigAllSettings(_custom_configs._canifier);
                Debug.Print("custom config finish");

            }
            else if (_btns[5] && !_btnsLast[5])
            {

                Debug.Print("factory default start");
                _talon.ConfigFactoryDefault();
                _victor.ConfigFactoryDefault();
                //_pigeon.ConfigFactoryDefault();
                _canifier.ConfigFactoryDefault();
                Debug.Print("factory default finish");


            }
            for(int i = 0; i < _btns.Length; i++)
            {
                _btnsLast[i] = _btns[i];
            }
            

        }

        /** throw all the gamepad buttons into an array */
        void FillBtns(ref bool[] btns)
        {
            for (uint i = 1; i < btns.Length; ++i)
            {
                btns[i] = _gamepad.GetButton(i);
                if (btns[i])
                {
                    Debug.Print(btns[i].ToString() + " " + i.ToString());
                }

            }
        }
    }
}