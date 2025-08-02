using System;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// A serializable data structure representing a screen resulotion. 
    /// </summary>
    [Serializable]
    public struct ResolutionOption : IEquatable<Resolution>, IEquatable<ResolutionOption>, IComparable<ResolutionOption>
    {
        public int Width => _width;
        public int Height => _height;
        public int RefreshRate => _refreshRate;
        public Vector2Int Dimensions => new Vector2Int(_width, _height);

        [SerializeField]
        private int _width;

        [SerializeField]
        private int _height;

        [SerializeField]
        private int _refreshRate;


        public ResolutionOption(Resolution resolution)
        {
            _width = resolution.width;
            _height = resolution.height;
            _refreshRate = GetRoundedRefreshRate(resolution.refreshRate);
        }

        public bool IsSupported()
        {
            if (_width <= 0) return false;
            if (_height <= 0) return false;
            if (_refreshRate <= 0) return false;

            Resolution[] resoltions = Screen.resolutions;
            for (int i = 0; i < resoltions.Length; i++)
            {
                if (_width == resoltions[i].width && _height == resoltions[i].height && _refreshRate == GetRoundedRefreshRate(resoltions[i].refreshRate))
                {
                    return true;
                }
            }

            return false;
        }

        public void Apply(FullScreenMode fullscreenMode)
        {
#if !UNITY_EDITOR
        UnityEngine.Assertions.Assert.IsTrue(IsSupported());
#endif

            if (GetCurrentResolution() != this || fullscreenMode != Screen.fullScreenMode)
            {
                Screen.SetResolution(_width, _height, fullscreenMode, _refreshRate);
                Canvas.ForceUpdateCanvases();
            }
        }

        public static ResolutionOption GetFromString(string value)
        {
            int xIndex = value.IndexOf(" x ");
            int bracketIndex = value.IndexOf(" (");

            int width = int.Parse(value.Substring(0, xIndex));
            int height = int.Parse(value.Substring(xIndex + 3, bracketIndex - xIndex - 3));
            int refreshRate = int.Parse(value.Substring(bracketIndex + 2, value.Length - bracketIndex - 5));

            return new ResolutionOption
            {
                _height = height,
                _width = width,
                _refreshRate = GetRoundedRefreshRate(refreshRate)
            };
        }

        public static ResolutionOption GetLargestSupportedResolution()
        {
            ResolutionOption maxResolution = new ResolutionOption();

            Resolution[] resoltions = Screen.resolutions;
            for (int i = 0; i < resoltions.Length; i++)
            {
                ResolutionOption resolution = new ResolutionOption(resoltions[i]);
                if (resolution > maxResolution)
                {
                    maxResolution = resolution;
                }
            }

            if (maxResolution.IsSupported())
            {
                return maxResolution;
            }

            return new ResolutionOption(Screen.currentResolution);
        }

        public static ResolutionOption GetCurrentResolution()
        {
            return new ResolutionOption(Screen.currentResolution);
        }

        public static int CountSupportedResolutions()
        {
            return Screen.resolutions.Length;
        }

        public static ResolutionOption[] GetSupportedResolutions()
        {
            SortedList<ResolutionOption> supportedResolutions = new SortedList<ResolutionOption>();
            Resolution[] resoltions = Screen.resolutions;

            for (int i = 0; i < resoltions.Length; i++)
            {
                ResolutionOption resoution = new ResolutionOption(resoltions[i]);
                if (!supportedResolutions.Contains(resoution))
                {
                    supportedResolutions.Add(resoution);
                }
            }

            return supportedResolutions.ToArray();
        }

        public static ResolutionOption GetNearestSupportedResolution(ResolutionOption resolution)
        {
            int minDist = int.MaxValue;
            ResolutionOption nearest = resolution;

            Resolution[] resoltions = Screen.resolutions;

            for (int i = 0; i < resoltions.Length; i++)
            {
                ResolutionOption option = new ResolutionOption(resoltions[i]);
                int dist = Mathf.Abs(option._width - resolution._width) + Mathf.Abs(option._height - resolution._height);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = option;
                }
                else if (dist == minDist)
                {
                    if (Mathf.Abs(resolution._refreshRate - option._refreshRate) < Mathf.Abs(resolution._refreshRate - nearest._refreshRate))
                    {
                        nearest = option;
                    }
                }
            }

            return nearest;
        }

        public bool Equals(Resolution other)
        {
            return _width == other.width && _height == other.height && _refreshRate == other.refreshRate;
        }

        public override bool Equals(object obj)
        {
            if (obj is ResolutionOption)
            {
                return Equals((ResolutionOption)obj);
            }
            return false;
        }

        public bool Equals(ResolutionOption other)
        {
            return _width == other._width && _height == other._height && _refreshRate == other._refreshRate;
        }

        public int CompareTo(ResolutionOption other)
        {
            if (_width * _height > other._width * other._height) return 1;
            if (_width * _height == other._width * other._height)
            {
                if (_width > other._width) return 1;

                return _refreshRate.CompareTo(other._refreshRate);
            }
            return -1;
        }

        public override string ToString()
        {
            return _width + " x " + _height + " (" + _refreshRate + "Hz)";
        }

        public static bool operator ==(ResolutionOption x, ResolutionOption y)
        {
            return x._width == y._width && x._height == y._height && x._refreshRate == y._refreshRate;
        }

        public static bool operator !=(ResolutionOption x, ResolutionOption y)
        {
            return x._width != y._width || x._height != y._height || x._refreshRate != y._refreshRate;
        }

        public static bool operator >(ResolutionOption x, ResolutionOption y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <(ResolutionOption x, ResolutionOption y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >=(ResolutionOption x, ResolutionOption y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static bool operator <=(ResolutionOption x, ResolutionOption y)
        {
            return x.CompareTo(y) <= 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _width;
                hashCode = (hashCode * 397) ^ _height;
                hashCode = (hashCode * 397) ^ _refreshRate;
                return hashCode;
            }
        }

        static int GetRoundedRefreshRate(int rate)
        {
            switch (rate)
            {
                case 23: return 24;
                case 29: return 30;
                case 47: return 48;
                case 59: return 60;
                case 75:
                case 74: return 75;
                case 164:
                case 165: return 165;
            }

            return (rate > 60 && rate % 2 > 0) ? rate + 1 : rate;
        }
    }
}