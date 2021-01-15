using RoboRemote.Classes;
using RoboRemote.Interfaces;
using SimpleModbus;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace RoboRemote.ViewModel
{
    public class ModbusItemViewModel : INotifyPropertyChanged, IItem
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        //{
        //    if (Object.Equals(storage, value))
        //        return false;

        //    storage = value;
        //    OnPropertyChanged(propertyName);
        //    return true;
        //}

        private SocketManagerNS.SocketManager Socket { get; }
        private SimpleModbusTCP ModbusTCP { get; }
        private TM_Comms.ModbusDictionary.MobusValue ModbusValue { get; set; }


        private string _value;
        public string Name { get; }
        public string Value { get => _value; set { _value = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value")); } }
        public string Type { get; }
        public string Addr { get; }
        public string Access { get; }

        public bool IsWritable { get; }

        public Command WriteItem { get; }
        public Command EditItem { get; }

        public IItem Instance => this;

        public ModbusItemViewModel(string name, TM_Comms.ModbusDictionary.MobusValue modbusValue, SocketManagerNS.SocketManager socket)
        {

            Name = name;
            ModbusValue = modbusValue;
            Socket = socket;

            ModbusTCP = new SimpleModbusTCP(Socket);

            Type = ModbusValue.Type.ToString().ToLower();
            Addr = $"{ModbusValue.Addr}\r\nx{ModbusValue.Addr:X}";
            Access = ModbusValue.Access.ToString();

            IsWritable = ModbusValue.Access == TM_Comms.ModbusDictionary.MobusValue.AccessTypes.RW || ModbusValue.Access == TM_Comms.ModbusDictionary.MobusValue.AccessTypes.W;

            WriteItem = new Command(() =>
            {
                Write("true");
            });

            EditItem = new Command(() =>
            {
                Edit();
            });
        }

        public async void Write(string value)
        {
            if (ModbusValue.Type == TM_Comms.ModbusDictionary.MobusValue.DataTypes.Coil)
            {
                if (StaticUtils.Regex.CheckIsTrue(value))
                    value = "true";
                else if (StaticUtils.Regex.CheckIsFalse(value))
                    value = "false";

                if (bool.TryParse(value, out bool res))
                {
                    if (!ModbusTCP.WriteSingleCoil(ModbusValue.Addr, res))
                        await Application.Current.MainPage.DisplayAlert("Modbus write error!", $"Could not write value: {res} to: {ModbusValue.Addr}", "OK");
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Parse error!", $"Could not parse value: {value}", "OK");
            }
            else if (ModbusValue.Type == TM_Comms.ModbusDictionary.MobusValue.DataTypes.Int16)
            {

                if (Int16.TryParse(value, out short res))
                {
                    if (!ModbusTCP.SetInt16(ModbusValue.Addr, new Int16[] { res }))
                        await Application.Current.MainPage.DisplayAlert("Modbus write error!", $"Could not write value: {res} to: {ModbusValue.Addr}", "OK");
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Parse error!", $"Could not parse value: {value}", "OK");

            }
            else if (ModbusValue.Type == TM_Comms.ModbusDictionary.MobusValue.DataTypes.Int32)
            {

                if (Int32.TryParse(value, out int res))
                {
                    if (!ModbusTCP.SetInt32(ModbusValue.Addr, new Int32[] { res }))
                        await Application.Current.MainPage.DisplayAlert("Modbus write error!", $"Could not write value: {res} to: {ModbusValue.Addr}", "OK");
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Parse error!", $"Could not parse value: {value}", "OK");

            }
            else if (ModbusValue.Type == TM_Comms.ModbusDictionary.MobusValue.DataTypes.Float)
            {

                if (float.TryParse(value, out float res))
                {
                    if (!ModbusTCP.SetFloat(ModbusValue.Addr, new float[] { res }))
                        await Application.Current.MainPage.DisplayAlert("Modbus write error!", $"Could not write value: {res} to: {ModbusValue.Addr}", "OK");
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Parse error!", $"Could not parse value: {value}", "OK");

            }
        }

        public async void Edit()
        {
            Keyboard k = Keyboard.Default;
            if (ModbusValue.Type != TM_Comms.ModbusDictionary.MobusValue.DataTypes.String && ModbusValue.Type != TM_Comms.ModbusDictionary.MobusValue.DataTypes.Coil)
            {
                k = Keyboard.Numeric;
            }

            string res = await Application.Current.MainPage.DisplayPromptAsync($"{Name}", "New Value", initialValue: $"{Value}", keyboard: k);

            if (res != null)
                Write(res);
        }
        public string Read()
        {
            if (Socket.IsConnected)
            {
                if (ModbusValue.Type == TM_Comms.ModbusDictionary.MobusValue.DataTypes.Input)
                    return Value = ModbusTCP.ReadDiscreteInput(ModbusValue.Addr).ToString();
                if (ModbusValue.Type == TM_Comms.ModbusDictionary.MobusValue.DataTypes.Coil)
                    return Value = ModbusTCP.ReadCoils(ModbusValue.Addr).ToString();
                if (ModbusValue.Type == TM_Comms.ModbusDictionary.MobusValue.DataTypes.Int16)
                    return Value = ModbusTCP.GetInt16(ModbusValue.Addr).ToString();
                if (ModbusValue.Type == TM_Comms.ModbusDictionary.MobusValue.DataTypes.Int32)
                    return Value = ModbusTCP.GetInt32(ModbusValue.Addr).ToString();
                if (ModbusValue.Type == TM_Comms.ModbusDictionary.MobusValue.DataTypes.Float)
                    return Value = ModbusTCP.GetFloat(ModbusValue.Addr).ToString();
            }
            return "";
        }
    }
}
