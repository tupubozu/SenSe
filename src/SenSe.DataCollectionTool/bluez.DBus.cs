using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Tmds.DBus.Connection.DynamicAssemblyName)]
namespace bluez.DBus
{
    [DBusInterface("org.freedesktop.DBus.ObjectManager")]
    interface IObjectManager : IDBusObject
    {
        Task<IDictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>>> GetManagedObjectsAsync();
        Task<IDisposable> WatchInterfacesAddedAsync(Action<(ObjectPath @object, IDictionary<string, IDictionary<string, object>> interfaces)> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchInterfacesRemovedAsync(Action<(ObjectPath @object, string[] interfaces)> handler, Action<Exception> onError = null);
    }

    [DBusInterface("org.bluez.AgentManager1")]
    interface IAgentManager1 : IDBusObject
    {
        Task RegisterAgentAsync(ObjectPath Agent, string Capability);
        Task UnregisterAgentAsync(ObjectPath Agent);
        Task RequestDefaultAgentAsync(ObjectPath Agent);
    }

    [DBusInterface("org.bluez.ProfileManager1")]
    interface IProfileManager1 : IDBusObject
    {
        Task RegisterProfileAsync(ObjectPath Profile, string UUID, IDictionary<string, object> Options);
        Task UnregisterProfileAsync(ObjectPath Profile);
    }

    [DBusInterface("org.bluez.Adapter1")]
    interface IAdapter1 : IDBusObject
    {
        Task StartDiscoveryAsync();
        Task SetDiscoveryFilterAsync(IDictionary<string, object> Properties);
        Task StopDiscoveryAsync();
        Task RemoveDeviceAsync(ObjectPath Device);
        Task<string[]> GetDiscoveryFiltersAsync();
        Task<T> GetAsync<T>(string prop);
        Task<Adapter1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class Adapter1Properties
    {
        private string _Address = default(string);
        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                _Address = (value);
            }
        }

        private string _AddressType = default(string);
        public string AddressType
        {
            get
            {
                return _AddressType;
            }

            set
            {
                _AddressType = (value);
            }
        }

        private string _Name = default(string);
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = (value);
            }
        }

        private string _Alias = default(string);
        public string Alias
        {
            get
            {
                return _Alias;
            }

            set
            {
                _Alias = (value);
            }
        }

        private uint _Class = default(uint);
        public uint Class
        {
            get
            {
                return _Class;
            }

            set
            {
                _Class = (value);
            }
        }

        private bool _Powered = default(bool);
        public bool Powered
        {
            get
            {
                return _Powered;
            }

            set
            {
                _Powered = (value);
            }
        }

        private bool _Discoverable = default(bool);
        public bool Discoverable
        {
            get
            {
                return _Discoverable;
            }

            set
            {
                _Discoverable = (value);
            }
        }

        private uint _DiscoverableTimeout = default(uint);
        public uint DiscoverableTimeout
        {
            get
            {
                return _DiscoverableTimeout;
            }

            set
            {
                _DiscoverableTimeout = (value);
            }
        }

        private bool _Pairable = default(bool);
        public bool Pairable
        {
            get
            {
                return _Pairable;
            }

            set
            {
                _Pairable = (value);
            }
        }

        private uint _PairableTimeout = default(uint);
        public uint PairableTimeout
        {
            get
            {
                return _PairableTimeout;
            }

            set
            {
                _PairableTimeout = (value);
            }
        }

        private bool _Discovering = default(bool);
        public bool Discovering
        {
            get
            {
                return _Discovering;
            }

            set
            {
                _Discovering = (value);
            }
        }

        private string[] _UUIDs = default(string[]);
        public string[] UUIDs
        {
            get
            {
                return _UUIDs;
            }

            set
            {
                _UUIDs = (value);
            }
        }

        private string _Modalias = default(string);
        public string Modalias
        {
            get
            {
                return _Modalias;
            }

            set
            {
                _Modalias = (value);
            }
        }

        private string[] _Roles = default(string[]);
        public string[] Roles
        {
            get
            {
                return _Roles;
            }

            set
            {
                _Roles = (value);
            }
        }

        private string[] _ExperimentalFeatures = default(string[]);
        public string[] ExperimentalFeatures
        {
            get
            {
                return _ExperimentalFeatures;
            }

            set
            {
                _ExperimentalFeatures = (value);
            }
        }
    }

    static class Adapter1Extensions
    {
        public static Task<string> GetAddressAsync(this IAdapter1 o) => o.GetAsync<string>("Address");
        public static Task<string> GetAddressTypeAsync(this IAdapter1 o) => o.GetAsync<string>("AddressType");
        public static Task<string> GetNameAsync(this IAdapter1 o) => o.GetAsync<string>("Name");
        public static Task<string> GetAliasAsync(this IAdapter1 o) => o.GetAsync<string>("Alias");
        public static Task<uint> GetClassAsync(this IAdapter1 o) => o.GetAsync<uint>("Class");
        public static Task<bool> GetPoweredAsync(this IAdapter1 o) => o.GetAsync<bool>("Powered");
        public static Task<bool> GetDiscoverableAsync(this IAdapter1 o) => o.GetAsync<bool>("Discoverable");
        public static Task<uint> GetDiscoverableTimeoutAsync(this IAdapter1 o) => o.GetAsync<uint>("DiscoverableTimeout");
        public static Task<bool> GetPairableAsync(this IAdapter1 o) => o.GetAsync<bool>("Pairable");
        public static Task<uint> GetPairableTimeoutAsync(this IAdapter1 o) => o.GetAsync<uint>("PairableTimeout");
        public static Task<bool> GetDiscoveringAsync(this IAdapter1 o) => o.GetAsync<bool>("Discovering");
        public static Task<string[]> GetUUIDsAsync(this IAdapter1 o) => o.GetAsync<string[]>("UUIDs");
        public static Task<string> GetModaliasAsync(this IAdapter1 o) => o.GetAsync<string>("Modalias");
        public static Task<string[]> GetRolesAsync(this IAdapter1 o) => o.GetAsync<string[]>("Roles");
        public static Task<string[]> GetExperimentalFeaturesAsync(this IAdapter1 o) => o.GetAsync<string[]>("ExperimentalFeatures");
        public static Task SetAliasAsync(this IAdapter1 o, string val) => o.SetAsync("Alias", val);
        public static Task SetPoweredAsync(this IAdapter1 o, bool val) => o.SetAsync("Powered", val);
        public static Task SetDiscoverableAsync(this IAdapter1 o, bool val) => o.SetAsync("Discoverable", val);
        public static Task SetDiscoverableTimeoutAsync(this IAdapter1 o, uint val) => o.SetAsync("DiscoverableTimeout", val);
        public static Task SetPairableAsync(this IAdapter1 o, bool val) => o.SetAsync("Pairable", val);
        public static Task SetPairableTimeoutAsync(this IAdapter1 o, uint val) => o.SetAsync("PairableTimeout", val);
    }

    [DBusInterface("org.bluez.GattManager1")]
    interface IGattManager1 : IDBusObject
    {
        Task RegisterApplicationAsync(ObjectPath Application, IDictionary<string, object> Options);
        Task UnregisterApplicationAsync(ObjectPath Application);
    }

    [DBusInterface("org.bluez.Media1")]
    interface IMedia1 : IDBusObject
    {
        Task RegisterEndpointAsync(ObjectPath Endpoint, IDictionary<string, object> Properties);
        Task UnregisterEndpointAsync(ObjectPath Endpoint);
        Task RegisterPlayerAsync(ObjectPath Player, IDictionary<string, object> Properties);
        Task UnregisterPlayerAsync(ObjectPath Player);
        Task RegisterApplicationAsync(ObjectPath Application, IDictionary<string, object> Options);
        Task UnregisterApplicationAsync(ObjectPath Application);
    }

    [DBusInterface("org.bluez.NetworkServer1")]
    interface INetworkServer1 : IDBusObject
    {
        Task RegisterAsync(string Uuid, string Bridge);
        Task UnregisterAsync(string Uuid);
    }

    [DBusInterface("org.bluez.LEAdvertisingManager1")]
    interface ILEAdvertisingManager1 : IDBusObject
    {
        Task RegisterAdvertisementAsync(ObjectPath Advertisement, IDictionary<string, object> Options);
        Task UnregisterAdvertisementAsync(ObjectPath Service);
        Task<T> GetAsync<T>(string prop);
        Task<LEAdvertisingManager1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class LEAdvertisingManager1Properties
    {
        private byte _ActiveInstances = default(byte);
        public byte ActiveInstances
        {
            get
            {
                return _ActiveInstances;
            }

            set
            {
                _ActiveInstances = (value);
            }
        }

        private byte _SupportedInstances = default(byte);
        public byte SupportedInstances
        {
            get
            {
                return _SupportedInstances;
            }

            set
            {
                _SupportedInstances = (value);
            }
        }

        private string[] _SupportedIncludes = default(string[]);
        public string[] SupportedIncludes
        {
            get
            {
                return _SupportedIncludes;
            }

            set
            {
                _SupportedIncludes = (value);
            }
        }

        private string[] _SupportedSecondaryChannels = default(string[]);
        public string[] SupportedSecondaryChannels
        {
            get
            {
                return _SupportedSecondaryChannels;
            }

            set
            {
                _SupportedSecondaryChannels = (value);
            }
        }
    }

    static class LEAdvertisingManager1Extensions
    {
        public static Task<byte> GetActiveInstancesAsync(this ILEAdvertisingManager1 o) => o.GetAsync<byte>("ActiveInstances");
        public static Task<byte> GetSupportedInstancesAsync(this ILEAdvertisingManager1 o) => o.GetAsync<byte>("SupportedInstances");
        public static Task<string[]> GetSupportedIncludesAsync(this ILEAdvertisingManager1 o) => o.GetAsync<string[]>("SupportedIncludes");
        public static Task<string[]> GetSupportedSecondaryChannelsAsync(this ILEAdvertisingManager1 o) => o.GetAsync<string[]>("SupportedSecondaryChannels");
    }

    [DBusInterface("org.bluez.Device1")]
    interface IDevice1 : IDBusObject
    {
        Task DisconnectAsync();
        Task ConnectAsync();
        Task ConnectProfileAsync(string UUID);
        Task DisconnectProfileAsync(string UUID);
        Task PairAsync();
        Task CancelPairingAsync();
        Task<T> GetAsync<T>(string prop);
        Task<Device1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class Device1Properties
    {
        private string _Address = default(string);
        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                _Address = (value);
            }
        }

        private string _AddressType = default(string);
        public string AddressType
        {
            get
            {
                return _AddressType;
            }

            set
            {
                _AddressType = (value);
            }
        }

        private string _Name = default(string);
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = (value);
            }
        }

        private string _Alias = default(string);
        public string Alias
        {
            get
            {
                return _Alias;
            }

            set
            {
                _Alias = (value);
            }
        }

        private uint _Class = default(uint);
        public uint Class
        {
            get
            {
                return _Class;
            }

            set
            {
                _Class = (value);
            }
        }

        private ushort _Appearance = default(ushort);
        public ushort Appearance
        {
            get
            {
                return _Appearance;
            }

            set
            {
                _Appearance = (value);
            }
        }

        private string _Icon = default(string);
        public string Icon
        {
            get
            {
                return _Icon;
            }

            set
            {
                _Icon = (value);
            }
        }

        private bool _Paired = default(bool);
        public bool Paired
        {
            get
            {
                return _Paired;
            }

            set
            {
                _Paired = (value);
            }
        }

        private bool _Bonded = default(bool);
        public bool Bonded
        {
            get
            {
                return _Bonded;
            }

            set
            {
                _Bonded = (value);
            }
        }

        private bool _Trusted = default(bool);
        public bool Trusted
        {
            get
            {
                return _Trusted;
            }

            set
            {
                _Trusted = (value);
            }
        }

        private bool _Blocked = default(bool);
        public bool Blocked
        {
            get
            {
                return _Blocked;
            }

            set
            {
                _Blocked = (value);
            }
        }

        private bool _LegacyPairing = default(bool);
        public bool LegacyPairing
        {
            get
            {
                return _LegacyPairing;
            }

            set
            {
                _LegacyPairing = (value);
            }
        }

        private short _RSSI = default(short);
        public short RSSI
        {
            get
            {
                return _RSSI;
            }

            set
            {
                _RSSI = (value);
            }
        }

        private bool _Connected = default(bool);
        public bool Connected
        {
            get
            {
                return _Connected;
            }

            set
            {
                _Connected = (value);
            }
        }

        private string[] _UUIDs = default(string[]);
        public string[] UUIDs
        {
            get
            {
                return _UUIDs;
            }

            set
            {
                _UUIDs = (value);
            }
        }

        private string _Modalias = default(string);
        public string Modalias
        {
            get
            {
                return _Modalias;
            }

            set
            {
                _Modalias = (value);
            }
        }

        private ObjectPath _Adapter = default(ObjectPath);
        public ObjectPath Adapter
        {
            get
            {
                return _Adapter;
            }

            set
            {
                _Adapter = (value);
            }
        }

        private IDictionary<ushort, object> _ManufacturerData = default(IDictionary<ushort, object>);
        public IDictionary<ushort, object> ManufacturerData
        {
            get
            {
                return _ManufacturerData;
            }

            set
            {
                _ManufacturerData = (value);
            }
        }

        private IDictionary<string, object> _ServiceData = default(IDictionary<string, object>);
        public IDictionary<string, object> ServiceData
        {
            get
            {
                return _ServiceData;
            }

            set
            {
                _ServiceData = (value);
            }
        }

        private short _TxPower = default(short);
        public short TxPower
        {
            get
            {
                return _TxPower;
            }

            set
            {
                _TxPower = (value);
            }
        }

        private bool _ServicesResolved = default(bool);
        public bool ServicesResolved
        {
            get
            {
                return _ServicesResolved;
            }

            set
            {
                _ServicesResolved = (value);
            }
        }

        private bool _WakeAllowed = default(bool);
        public bool WakeAllowed
        {
            get
            {
                return _WakeAllowed;
            }

            set
            {
                _WakeAllowed = (value);
            }
        }
    }

    static class Device1Extensions
    {
        public static Task<string> GetAddressAsync(this IDevice1 o) => o.GetAsync<string>("Address");
        public static Task<string> GetAddressTypeAsync(this IDevice1 o) => o.GetAsync<string>("AddressType");
        public static Task<string> GetNameAsync(this IDevice1 o) => o.GetAsync<string>("Name");
        public static Task<string> GetAliasAsync(this IDevice1 o) => o.GetAsync<string>("Alias");
        public static Task<uint> GetClassAsync(this IDevice1 o) => o.GetAsync<uint>("Class");
        public static Task<ushort> GetAppearanceAsync(this IDevice1 o) => o.GetAsync<ushort>("Appearance");
        public static Task<string> GetIconAsync(this IDevice1 o) => o.GetAsync<string>("Icon");
        public static Task<bool> GetPairedAsync(this IDevice1 o) => o.GetAsync<bool>("Paired");
        public static Task<bool> GetBondedAsync(this IDevice1 o) => o.GetAsync<bool>("Bonded");
        public static Task<bool> GetTrustedAsync(this IDevice1 o) => o.GetAsync<bool>("Trusted");
        public static Task<bool> GetBlockedAsync(this IDevice1 o) => o.GetAsync<bool>("Blocked");
        public static Task<bool> GetLegacyPairingAsync(this IDevice1 o) => o.GetAsync<bool>("LegacyPairing");
        public static Task<short> GetRSSIAsync(this IDevice1 o) => o.GetAsync<short>("RSSI");
        public static Task<bool> GetConnectedAsync(this IDevice1 o) => o.GetAsync<bool>("Connected");
        public static Task<string[]> GetUUIDsAsync(this IDevice1 o) => o.GetAsync<string[]>("UUIDs");
        public static Task<string> GetModaliasAsync(this IDevice1 o) => o.GetAsync<string>("Modalias");
        public static Task<ObjectPath> GetAdapterAsync(this IDevice1 o) => o.GetAsync<ObjectPath>("Adapter");
        public static Task<IDictionary<ushort, object>> GetManufacturerDataAsync(this IDevice1 o) => o.GetAsync<IDictionary<ushort, object>>("ManufacturerData");
        public static Task<IDictionary<string, object>> GetServiceDataAsync(this IDevice1 o) => o.GetAsync<IDictionary<string, object>>("ServiceData");
        public static Task<short> GetTxPowerAsync(this IDevice1 o) => o.GetAsync<short>("TxPower");
        public static Task<bool> GetServicesResolvedAsync(this IDevice1 o) => o.GetAsync<bool>("ServicesResolved");
        public static Task<bool> GetWakeAllowedAsync(this IDevice1 o) => o.GetAsync<bool>("WakeAllowed");
        public static Task SetAliasAsync(this IDevice1 o, string val) => o.SetAsync("Alias", val);
        public static Task SetTrustedAsync(this IDevice1 o, bool val) => o.SetAsync("Trusted", val);
        public static Task SetBlockedAsync(this IDevice1 o, bool val) => o.SetAsync("Blocked", val);
        public static Task SetWakeAllowedAsync(this IDevice1 o, bool val) => o.SetAsync("WakeAllowed", val);
    }

    [DBusInterface("org.bluez.MediaControl1")]
    interface IMediaControl1 : IDBusObject
    {
        Task PlayAsync();
        Task PauseAsync();
        Task StopAsync();
        Task NextAsync();
        Task PreviousAsync();
        Task VolumeUpAsync();
        Task VolumeDownAsync();
        Task FastForwardAsync();
        Task RewindAsync();
        Task<T> GetAsync<T>(string prop);
        Task<MediaControl1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class MediaControl1Properties
    {
        private bool _Connected = default(bool);
        public bool Connected
        {
            get
            {
                return _Connected;
            }

            set
            {
                _Connected = (value);
            }
        }

        private ObjectPath _Player = default(ObjectPath);
        public ObjectPath Player
        {
            get
            {
                return _Player;
            }

            set
            {
                _Player = (value);
            }
        }
    }

    static class MediaControl1Extensions
    {
        public static Task<bool> GetConnectedAsync(this IMediaControl1 o) => o.GetAsync<bool>("Connected");
        public static Task<ObjectPath> GetPlayerAsync(this IMediaControl1 o) => o.GetAsync<ObjectPath>("Player");
    }

    [DBusInterface("org.bluez.MediaPlayer1")]
    interface IMediaPlayer1 : IDBusObject
    {
        Task PlayAsync();
        Task PauseAsync();
        Task StopAsync();
        Task NextAsync();
        Task PreviousAsync();
        Task FastForwardAsync();
        Task RewindAsync();
        Task PressAsync(byte AvcKey);
        Task HoldAsync(byte AvcKey);
        Task ReleaseAsync();
        Task<T> GetAsync<T>(string prop);
        Task<MediaPlayer1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class MediaPlayer1Properties
    {
        private string _Name = default(string);
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = (value);
            }
        }

        private string _Type = default(string);
        public string Type
        {
            get
            {
                return _Type;
            }

            set
            {
                _Type = (value);
            }
        }

        private string _Subtype = default(string);
        public string Subtype
        {
            get
            {
                return _Subtype;
            }

            set
            {
                _Subtype = (value);
            }
        }

        private uint _Position = default(uint);
        public uint Position
        {
            get
            {
                return _Position;
            }

            set
            {
                _Position = (value);
            }
        }

        private string _Status = default(string);
        public string Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = (value);
            }
        }

        private string _Equalizer = default(string);
        public string Equalizer
        {
            get
            {
                return _Equalizer;
            }

            set
            {
                _Equalizer = (value);
            }
        }

        private string _Repeat = default(string);
        public string Repeat
        {
            get
            {
                return _Repeat;
            }

            set
            {
                _Repeat = (value);
            }
        }

        private string _Shuffle = default(string);
        public string Shuffle
        {
            get
            {
                return _Shuffle;
            }

            set
            {
                _Shuffle = (value);
            }
        }

        private string _Scan = default(string);
        public string Scan
        {
            get
            {
                return _Scan;
            }

            set
            {
                _Scan = (value);
            }
        }

        private IDictionary<string, object> _Track = default(IDictionary<string, object>);
        public IDictionary<string, object> Track
        {
            get
            {
                return _Track;
            }

            set
            {
                _Track = (value);
            }
        }

        private ObjectPath _Device = default(ObjectPath);
        public ObjectPath Device
        {
            get
            {
                return _Device;
            }

            set
            {
                _Device = (value);
            }
        }

        private bool _Browsable = default(bool);
        public bool Browsable
        {
            get
            {
                return _Browsable;
            }

            set
            {
                _Browsable = (value);
            }
        }

        private bool _Searchable = default(bool);
        public bool Searchable
        {
            get
            {
                return _Searchable;
            }

            set
            {
                _Searchable = (value);
            }
        }

        private ObjectPath _Playlist = default(ObjectPath);
        public ObjectPath Playlist
        {
            get
            {
                return _Playlist;
            }

            set
            {
                _Playlist = (value);
            }
        }
    }

    static class MediaPlayer1Extensions
    {
        public static Task<string> GetNameAsync(this IMediaPlayer1 o) => o.GetAsync<string>("Name");
        public static Task<string> GetTypeAsync(this IMediaPlayer1 o) => o.GetAsync<string>("Type");
        public static Task<string> GetSubtypeAsync(this IMediaPlayer1 o) => o.GetAsync<string>("Subtype");
        public static Task<uint> GetPositionAsync(this IMediaPlayer1 o) => o.GetAsync<uint>("Position");
        public static Task<string> GetStatusAsync(this IMediaPlayer1 o) => o.GetAsync<string>("Status");
        public static Task<string> GetEqualizerAsync(this IMediaPlayer1 o) => o.GetAsync<string>("Equalizer");
        public static Task<string> GetRepeatAsync(this IMediaPlayer1 o) => o.GetAsync<string>("Repeat");
        public static Task<string> GetShuffleAsync(this IMediaPlayer1 o) => o.GetAsync<string>("Shuffle");
        public static Task<string> GetScanAsync(this IMediaPlayer1 o) => o.GetAsync<string>("Scan");
        public static Task<IDictionary<string, object>> GetTrackAsync(this IMediaPlayer1 o) => o.GetAsync<IDictionary<string, object>>("Track");
        public static Task<ObjectPath> GetDeviceAsync(this IMediaPlayer1 o) => o.GetAsync<ObjectPath>("Device");
        public static Task<bool> GetBrowsableAsync(this IMediaPlayer1 o) => o.GetAsync<bool>("Browsable");
        public static Task<bool> GetSearchableAsync(this IMediaPlayer1 o) => o.GetAsync<bool>("Searchable");
        public static Task<ObjectPath> GetPlaylistAsync(this IMediaPlayer1 o) => o.GetAsync<ObjectPath>("Playlist");
        public static Task SetEqualizerAsync(this IMediaPlayer1 o, string val) => o.SetAsync("Equalizer", val);
        public static Task SetRepeatAsync(this IMediaPlayer1 o, string val) => o.SetAsync("Repeat", val);
        public static Task SetShuffleAsync(this IMediaPlayer1 o, string val) => o.SetAsync("Shuffle", val);
        public static Task SetScanAsync(this IMediaPlayer1 o, string val) => o.SetAsync("Scan", val);
    }

    [DBusInterface("org.bluez.MediaEndpoint1")]
    interface IMediaEndpoint1 : IDBusObject
    {
        Task SetConfigurationAsync(ObjectPath Endpoint, IDictionary<string, object> Properties);
        Task<T> GetAsync<T>(string prop);
        Task<MediaEndpoint1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class MediaEndpoint1Properties
    {
        private string _UUID = default(string);
        public string UUID
        {
            get
            {
                return _UUID;
            }

            set
            {
                _UUID = (value);
            }
        }

        private byte _Codec = default(byte);
        public byte Codec
        {
            get
            {
                return _Codec;
            }

            set
            {
                _Codec = (value);
            }
        }

        private byte[] _Capabilities = default(byte[]);
        public byte[] Capabilities
        {
            get
            {
                return _Capabilities;
            }

            set
            {
                _Capabilities = (value);
            }
        }

        private ObjectPath _Device = default(ObjectPath);
        public ObjectPath Device
        {
            get
            {
                return _Device;
            }

            set
            {
                _Device = (value);
            }
        }

        private bool _DelayReporting = default(bool);
        public bool DelayReporting
        {
            get
            {
                return _DelayReporting;
            }

            set
            {
                _DelayReporting = (value);
            }
        }
    }

    static class MediaEndpoint1Extensions
    {
        public static Task<string> GetUUIDAsync(this IMediaEndpoint1 o) => o.GetAsync<string>("UUID");
        public static Task<byte> GetCodecAsync(this IMediaEndpoint1 o) => o.GetAsync<byte>("Codec");
        public static Task<byte[]> GetCapabilitiesAsync(this IMediaEndpoint1 o) => o.GetAsync<byte[]>("Capabilities");
        public static Task<ObjectPath> GetDeviceAsync(this IMediaEndpoint1 o) => o.GetAsync<ObjectPath>("Device");
        public static Task<bool> GetDelayReportingAsync(this IMediaEndpoint1 o) => o.GetAsync<bool>("DelayReporting");
    }

    [DBusInterface("org.bluez.MediaTransport1")]
    interface IMediaTransport1 : IDBusObject
    {
        Task<(CloseSafeHandle fd, ushort mtuR, ushort mtuW)> AcquireAsync();
        Task<(CloseSafeHandle fd, ushort mtuR, ushort mtuW)> TryAcquireAsync();
        Task ReleaseAsync();
        Task<T> GetAsync<T>(string prop);
        Task<MediaTransport1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class MediaTransport1Properties
    {
        private ObjectPath _Device = default(ObjectPath);
        public ObjectPath Device
        {
            get
            {
                return _Device;
            }

            set
            {
                _Device = (value);
            }
        }

        private string _UUID = default(string);
        public string UUID
        {
            get
            {
                return _UUID;
            }

            set
            {
                _UUID = (value);
            }
        }

        private byte _Codec = default(byte);
        public byte Codec
        {
            get
            {
                return _Codec;
            }

            set
            {
                _Codec = (value);
            }
        }

        private byte[] _Configuration = default(byte[]);
        public byte[] Configuration
        {
            get
            {
                return _Configuration;
            }

            set
            {
                _Configuration = (value);
            }
        }

        private string _State = default(string);
        public string State
        {
            get
            {
                return _State;
            }

            set
            {
                _State = (value);
            }
        }

        private ushort _Delay = default(ushort);
        public ushort Delay
        {
            get
            {
                return _Delay;
            }

            set
            {
                _Delay = (value);
            }
        }

        private ushort _Volume = default(ushort);
        public ushort Volume
        {
            get
            {
                return _Volume;
            }

            set
            {
                _Volume = (value);
            }
        }
    }

    static class MediaTransport1Extensions
    {
        public static Task<ObjectPath> GetDeviceAsync(this IMediaTransport1 o) => o.GetAsync<ObjectPath>("Device");
        public static Task<string> GetUUIDAsync(this IMediaTransport1 o) => o.GetAsync<string>("UUID");
        public static Task<byte> GetCodecAsync(this IMediaTransport1 o) => o.GetAsync<byte>("Codec");
        public static Task<byte[]> GetConfigurationAsync(this IMediaTransport1 o) => o.GetAsync<byte[]>("Configuration");
        public static Task<string> GetStateAsync(this IMediaTransport1 o) => o.GetAsync<string>("State");
        public static Task<ushort> GetDelayAsync(this IMediaTransport1 o) => o.GetAsync<ushort>("Delay");
        public static Task<ushort> GetVolumeAsync(this IMediaTransport1 o) => o.GetAsync<ushort>("Volume");
        public static Task SetVolumeAsync(this IMediaTransport1 o, ushort val) => o.SetAsync("Volume", val);
    }

    [DBusInterface("org.bluez.Battery1")]
    interface IBattery1 : IDBusObject
    {
        Task<T> GetAsync<T>(string prop);
        Task<Battery1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class Battery1Properties
    {
        private byte _Percentage = default(byte);
        public byte Percentage
        {
            get
            {
                return _Percentage;
            }

            set
            {
                _Percentage = (value);
            }
        }
    }

    static class Battery1Extensions
    {
        public static Task<byte> GetPercentageAsync(this IBattery1 o) => o.GetAsync<byte>("Percentage");
    }

    [DBusInterface("org.bluez.GattService1")]
    interface IGattService1 : IDBusObject
    {
        Task<T> GetAsync<T>(string prop);
        Task<GattService1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class GattService1Properties
    {
        private string _UUID = default(string);
        public string UUID
        {
            get
            {
                return _UUID;
            }

            set
            {
                _UUID = (value);
            }
        }

        private ObjectPath _Device = default(ObjectPath);
        public ObjectPath Device
        {
            get
            {
                return _Device;
            }

            set
            {
                _Device = (value);
            }
        }

        private bool _Primary = default(bool);
        public bool Primary
        {
            get
            {
                return _Primary;
            }

            set
            {
                _Primary = (value);
            }
        }

        private ObjectPath[] _Includes = default(ObjectPath[]);
        public ObjectPath[] Includes
        {
            get
            {
                return _Includes;
            }

            set
            {
                _Includes = (value);
            }
        }
    }

    static class GattService1Extensions
    {
        public static Task<string> GetUUIDAsync(this IGattService1 o) => o.GetAsync<string>("UUID");
        public static Task<ObjectPath> GetDeviceAsync(this IGattService1 o) => o.GetAsync<ObjectPath>("Device");
        public static Task<bool> GetPrimaryAsync(this IGattService1 o) => o.GetAsync<bool>("Primary");
        public static Task<ObjectPath[]> GetIncludesAsync(this IGattService1 o) => o.GetAsync<ObjectPath[]>("Includes");
    }

    [DBusInterface("org.bluez.GattCharacteristic1")]
    interface IGattCharacteristic1 : IDBusObject
    {
        Task<byte[]> ReadValueAsync(IDictionary<string, object> Options);
        Task WriteValueAsync(byte[] Value, IDictionary<string, object> Options);
        Task<(CloseSafeHandle fd, ushort mtu)> AcquireWriteAsync(IDictionary<string, object> Options);
        Task<(CloseSafeHandle fd, ushort mtu)> AcquireNotifyAsync(IDictionary<string, object> Options);
        Task StartNotifyAsync();
        Task StopNotifyAsync();
        Task<T> GetAsync<T>(string prop);
        Task<GattCharacteristic1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class GattCharacteristic1Properties
    {
        private string _UUID = default(string);
        public string UUID
        {
            get
            {
                return _UUID;
            }

            set
            {
                _UUID = (value);
            }
        }

        private ObjectPath _Service = default(ObjectPath);
        public ObjectPath Service
        {
            get
            {
                return _Service;
            }

            set
            {
                _Service = (value);
            }
        }

        private byte[] _Value = default(byte[]);
        public byte[] Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = (value);
            }
        }

        private bool _Notifying = default(bool);
        public bool Notifying
        {
            get
            {
                return _Notifying;
            }

            set
            {
                _Notifying = (value);
            }
        }

        private string[] _Flags = default(string[]);
        public string[] Flags
        {
            get
            {
                return _Flags;
            }

            set
            {
                _Flags = (value);
            }
        }

        private bool _WriteAcquired = default(bool);
        public bool WriteAcquired
        {
            get
            {
                return _WriteAcquired;
            }

            set
            {
                _WriteAcquired = (value);
            }
        }

        private bool _NotifyAcquired = default(bool);
        public bool NotifyAcquired
        {
            get
            {
                return _NotifyAcquired;
            }

            set
            {
                _NotifyAcquired = (value);
            }
        }

        private ushort _MTU = default(ushort);
        public ushort MTU
        {
            get
            {
                return _MTU;
            }

            set
            {
                _MTU = (value);
            }
        }
    }

    static class GattCharacteristic1Extensions
    {
        public static Task<string> GetUUIDAsync(this IGattCharacteristic1 o) => o.GetAsync<string>("UUID");
        public static Task<ObjectPath> GetServiceAsync(this IGattCharacteristic1 o) => o.GetAsync<ObjectPath>("Service");
        public static Task<byte[]> GetValueAsync(this IGattCharacteristic1 o) => o.GetAsync<byte[]>("Value");
        public static Task<bool> GetNotifyingAsync(this IGattCharacteristic1 o) => o.GetAsync<bool>("Notifying");
        public static Task<string[]> GetFlagsAsync(this IGattCharacteristic1 o) => o.GetAsync<string[]>("Flags");
        public static Task<bool> GetWriteAcquiredAsync(this IGattCharacteristic1 o) => o.GetAsync<bool>("WriteAcquired");
        public static Task<bool> GetNotifyAcquiredAsync(this IGattCharacteristic1 o) => o.GetAsync<bool>("NotifyAcquired");
        public static Task<ushort> GetMTUAsync(this IGattCharacteristic1 o) => o.GetAsync<ushort>("MTU");
    }

    [DBusInterface("org.bluez.GattDescriptor1")]
    interface IGattDescriptor1 : IDBusObject
    {
        Task<byte[]> ReadValueAsync(IDictionary<string, object> Options);
        Task WriteValueAsync(byte[] Value, IDictionary<string, object> Options);
        Task<T> GetAsync<T>(string prop);
        Task<GattDescriptor1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class GattDescriptor1Properties
    {
        private string _UUID = default(string);
        public string UUID
        {
            get
            {
                return _UUID;
            }

            set
            {
                _UUID = (value);
            }
        }

        private ObjectPath _Characteristic = default(ObjectPath);
        public ObjectPath Characteristic
        {
            get
            {
                return _Characteristic;
            }

            set
            {
                _Characteristic = (value);
            }
        }

        private byte[] _Value = default(byte[]);
        public byte[] Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = (value);
            }
        }
    }

    static class GattDescriptor1Extensions
    {
        public static Task<string> GetUUIDAsync(this IGattDescriptor1 o) => o.GetAsync<string>("UUID");
        public static Task<ObjectPath> GetCharacteristicAsync(this IGattDescriptor1 o) => o.GetAsync<ObjectPath>("Characteristic");
        public static Task<byte[]> GetValueAsync(this IGattDescriptor1 o) => o.GetAsync<byte[]>("Value");
    }

    [DBusInterface("org.bluez.Input1")]
    interface IInput1 : IDBusObject
    {
        Task<T> GetAsync<T>(string prop);
        Task<Input1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class Input1Properties
    {
        private string _ReconnectMode = default(string);
        public string ReconnectMode
        {
            get
            {
                return _ReconnectMode;
            }

            set
            {
                _ReconnectMode = (value);
            }
        }
    }

    static class Input1Extensions
    {
        public static Task<string> GetReconnectModeAsync(this IInput1 o) => o.GetAsync<string>("ReconnectMode");
    }
}