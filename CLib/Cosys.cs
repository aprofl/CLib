using CLib.Infos;
using System.Collections.ObjectModel;

namespace CLib
{
    public class SystemInfo
    {
        /// <inheritdoc cref="Infos.Platforms"/>
        public Platforms Platforms => Platforms.Instance;

        /// <inheritdoc cref="Infos.Devices"/>
        public Devices Devices => Devices.Instance;
    }

    /// <summary>
    /// Comizoa System class
    /// </summary>
    public class Cosys
    {
        Device.Manager DeviceManager { get; set; } = new Device.Manager();

        
        /// <summary>
        /// System에 로드된 제품 목록
        /// </summary>
        public ReadOnlyCollection<Device.Info> Devices => DeviceManager.List;

        /// <inheritdoc cref="SystemInfo"/>
        internal SystemInfo Info = new SystemInfo();

        /// <summary>
        /// Property를 통해 입력되는 값이 즉시 적용 되도록 설정
        /// </summary>
        /// <example>
        /// <code>
        /// picker.Config.Unit.Value = 100;
        /// picker.Config.Unit.Set(); // 생략 가능
        /// </code></example>
        public bool IsPropertySetEnable { get; set; }

        /// <summary>
        /// 시스템을 초기화합니다.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>COMIZOA 제품을 검색하여 Device Class를 생성합니다.</item>
        /// </list>
        /// </remarks>
        /// <returns>초기화 성공 여부</returns>
        /// <example>
        /// <code>
        /// ComiSys.Init();
        /// ComiSys.Start();
        /// </code></example>
        public async Task<bool> Init()
            => await Task.Run(() =>
            {                
                DeviceManager.Init();
                //return devices.All(x=>x.state)
                return true;
            });


        /// <summary>
        /// System을 시작합니다.
        /// </summary>
        /// <remarks>
        /// <para><strong>EtherCAT</strong></para>
        /// <list type="bullet">
        /// <item>SW 버전이 호환 가능 상태인지 확인합니다.</item>
        /// <item>역삽입된 모듈이 있는지 확인합니다.</item>
        /// <item>드라이버가 Alarm 상태인 경우, 클리어합니다.</item>
        /// <item>Al-Status를 OP로 전환합니다.</item>
        /// </list>
        /// </remarks>
        /// <seealso cref="Init()"/>
        public void Start()
        {
            foreach (var dev in Devices)
                dev.Init();
        }

        /// <summary>
        /// System을 종료합니다.
        /// </summary>
        public bool End()
        {
            foreach (var d in Devices)
            {
                //var log = Log.Add(nameof(End)).S(d);
                try
                {
                    //d.Motion?.axisList.ForEach(x => x.Motion.Stop());
                    //d.UnloadDevice(log);
                }
                catch (Exception ex)
                {
                    //log.Compt(ex);
                    return false;
                }
            }

            //devices.Clear();
            return true;
        }
    }
        /// <summary>
        /// <para>System Initialize 상세 옵션입니다.</para>
        /// <para>일반적으로 사용하지 않습니다.</para>
        /// </summary>
        public class InitOption
        {
            /// <summary>
            /// cEIP 시스템 로드 시 마스터의 수
            /// </summary>
            /// <remarks>Scan TimeOut 시간을 결정합니다.</remarks>
            public static int ScanNodeCount = 2;
            //public static uint nodeScanTimeOut = 80;

            /// <summary>
            /// cEIP 시스템 초기화 시 cEIP / AllNet 구분
            /// </summary>
            /// <remarks>Slave의 이름을 결정합니다.</remarks>
            public static bool IsAllNet = false;

            /// <summary>
            /// DAQ(CP, SD) System 초기화 시 사용할 DLL을 결정합니다.
            /// </summary>
            /// <remarks>
            /// <list>
            /// <item>true : Cmmsdk.dll</item>
            /// <item>false : ComiDll.dll</item>
            /// </list>
            /// </remarks>
            public static bool IsDioDevLoadOnPulse = true;

            /// <summary>
            /// EtherCAT System 초기화 시 Driver의 IO Class를 생성할지 결정합니다.
            /// </summary>
            /// <remarks></remarks>
            public static bool MotionIoVisible = false;

            internal static bool isDioLoaded = false;

            //public static bool driverCheck = true;
        }
    }

