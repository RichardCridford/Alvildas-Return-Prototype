using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{

//	protected Dictionary<int, SeeThroughObj> objChangedList = new Dictionary<int, SeeThroughObj>();
//	[SerializeField] public List<SeeThroughObj> deleteObjList = new List<SeeThroughObj>();
//
//	[System.Serializable]
//	public  class SeeThroughObj
//	{
//		public int ID = 0;
//		public Color prevColor = Color.white;
//		public Renderer rend = null;
//	}
//	public float radius;
//	// Update is called once per frame
//	void Update()
//	{
//		Vector3 dir = (LevelController.Alvilda.Position - transform.position).normalized;
//		Ray ray = new Ray(transform.position, dir);
//		RaycastHit[] hits = Physics.SphereCastAll(ray, radius, 5000f);
//
//		for(int hitIndex = 0; hitIndex < hits.Length; hitIndex++)
//		{
//			RaycastHit hit = hits[hitIndex];
//			if (hit.collider)
//			{
//				if (!hit.collider.IsAlvilda())
//				{
//					if (!objChangedList.ContainsKey(hit.transform.GetInstanceID()))
//					{
//						Renderer r = hit.collider.gameObject.GetComponent<Renderer>();
//						if (r)
//						{
//							if (r.material)
//							{
//
//								Color c = r.material.GetColor("_Color");
//
//								SeeThroughObj sto = new SeeThroughObj()
//								{
//									ID = hit.transform.GetInstanceID(),
//									prevColor = c,
//									rend = r
//								};
//
//								c.a = 0.15f;
//								r.material.SetColor("_Color", c);
//
//								objChangedList.Add(sto.ID, sto);
//							}
//						}
//					}
//					else
//					{
//						continue;
//					}
//				}
//			}
//		}
//
//		Dictionary<int, SeeThroughObj> copy = new Dictionary<int, SeeThroughObj>(objChangedList);
//		deleteObjList.Clear();
//		string s = "";
//		//if (objChangedList.Count != hits.Length)
//		{
//			foreach(KeyValuePair<int, SeeThroughObj> entry in copy)
//			{
//				deleteObjList.Add(entry.Value);
//				s += entry.Value.rend.name + " : ";
//				bool found = false;
//				for(int hitIndex = 0; hitIndex < hits.Length; hitIndex++)
//				{
//					RaycastHit hit = hits[hitIndex];
//					if (hit.collider)
//					{
//						if (!hit.collider.IsAlvilda())
//						{
//							if (hit.transform.GetInstanceID() == entry.Key)
//							{
//								found = true;
//							}
//						}
//					}
//				}
//			
//				if (!found)
//				{
//					entry.Value.rend.material.SetColor("_Color", Color.white);
//					objChangedList.Remove(entry.Key);
//				}
//			}
//		}
//		if (s != "")
//		{
////			Debug.Log(s);
//		}
//	}
}
