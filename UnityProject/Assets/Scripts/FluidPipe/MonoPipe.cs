﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pipes
{
	public class MonoPipe : MonoBehaviour
	{
		private RegisterTile registerTile;
		public PipeData pipeData;
		public Matrix Matrix => registerTile.Matrix;
		public Vector3Int MatrixPos => registerTile.LocalPosition;
		public void Start()
		{
			EnsureInit();
		}

		private void EnsureInit()
		{
			if (registerTile == null)
			{
				registerTile = GetComponent<RegisterTile>();
			}

			registerTile.SetPipeData(pipeData);
			Vector2 searchVec = this.registerTile.LocalPosition.To2Int();
			pipeData.MonoPipe = this;
			pipeData.OnEnable();
		}

		void OnDisable()
		{
			pipeData.OnDisable();
		}

		void OnEnable()
		{

		}

		public virtual void TickUpdate()
		{
		}

	}

}
