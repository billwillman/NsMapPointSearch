using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace NsMapPointSearch
{

	public struct MapPoint
	{
		public MapPoint(int x, int y)
		{
			X = x;
			Y = y;
		}

		public void Reset()
		{
			X = 0;
			Y = 0;
		}

		public int X;
		public int Y;
	}

	// 地图跳转点
	public class MapJumpPoint
	{
		// 拥有地图
		public int OwnerMapId
		{
			get;
			set;
		}

		public MapPoint Position
		{
			get;
			set;
		}

		// 目标地图
		public int TargetMapId
		{
			get;
			set;
		}

		// 跳转的位置
		public MapPoint TargetPos
		{
			get;
			set;
		}

		internal void Reset()
		{
			OwnerMapId = -1;
			TargetMapId = -1;
			Position.Reset();
			TargetPos.Reset();
		}
	}

	// 搜索地图
	public static class MapSearch
	{
		private static void InitPool()
		{
			if (m_InitPool)
				return;
			m_InitPool = true;
			m_PointPool.Init(0);
		}

		private static MapJumpPoint CreateJumpPoint()
		{
			InitPool();
			MapJumpPoint ret = m_PointPool.GetObject();
			ret.Reset();
			return ret;
		}

		private static void InPool(MapJumpPoint point)
		{
			if (point == null)
				return;
			InitPool();
			point.Reset();
			m_PointPool.Store(point);
		}

		public static void AddMapPoint(int mapId, int posX, int posY, int jumpMapId, int jumpX, int jumpY)
		{
			List<MapJumpPoint> jumpList;
			if (!m_MapPoints.TryGetValue(mapId, out jumpList))
			{
				jumpList = new List<MapJumpPoint>();
				m_MapPoints.Add(mapId, jumpList);
			}

			MapJumpPoint jump = CreateJumpPoint();
			jump.OwnerMapId = mapId;
			jump.Position = new MapPoint(posX, posY);
			jump.TargetMapId = jumpMapId;
			jump.TargetPos = new MapPoint(jumpX, jumpY);
			jumpList.Add(jump);
		}

		public static void Clear()
		{
			var iter = m_MapPoints.GetEnumerator();
			while (iter.MoveNext())
			{
				var list = iter.Current.Value;
				if (list != null)
				{
					for (int i = 0; i < list.Count; ++i)
					{
						var item = list[i];
						InPool(item);
					}

					list.Clear();
				}
			}
			iter.Dispose();
			m_MapPoints.Clear();
		}

		public static List<MapJumpPoint> Search(int mapId, int targetMapId)
		{
			if (mapId < 0 || targetMapId < 0 || !m_MapPoints.ContainsKey(mapId))
				return null;

			// 广度搜索算法

			return null;
		}


		// 所有地图的跳转点管理
		private static Dictionary<int, List<MapJumpPoint>> m_MapPoints = new Dictionary<int, List<MapJumpPoint>>();

		// 池
		private static bool m_InitPool = false;
		private static ObjectPool<MapJumpPoint> m_PointPool = new ObjectPool<MapJumpPoint>(); 
	}

}