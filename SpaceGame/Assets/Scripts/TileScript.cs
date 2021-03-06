using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class TileScript : Photon.MonoBehaviour {

	public Toolbox.TileType tileType;
	public int tileNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[PunRPC] // adds the child to the parent across the whole network
	void Parenting(int child, int parent, bool worldPositionStays){
		PhotonView x = PhotonView.Find (child);
		PhotonView y = PhotonView.Find (parent);
		
		x.transform.SetParent(y.transform, worldPositionStays);
	}

	[PunRPC] // adds the enemy to the hex across the whole network
	void AddEnemy(int hex, int newEnemy){
		HexScript h = PhotonView.Find (hex).transform.GetComponent<HexScript>();
		EnemyScript e = PhotonView.Find (newEnemy).transform.GetComponent<EnemyScript>();
		h.enemiesOnHex.Add(e);
	}

	[PunRPC] // adds the home to the hex across the whole network
	void AddHome(int hex, int newEnemy){
		HexScript h = PhotonView.Find (hex).transform.GetComponent<HexScript>();
		EnemyScript e = PhotonView.Find (newEnemy).transform.GetComponent<EnemyScript>();
		e.homeHex = h;
		if(h.hexType == Toolbox.HexType.Garrison){
			
			if(!e.specials.Any (x => x == Toolbox.EnemySpecial.Fortified)){
				e.specials.Add(Toolbox.EnemySpecial.Fortified);
				e.transform.GetComponent<EnemyTooltip>().Start();
			}
		}
	}
	
	public void SetEnemies(){
		GameObject greenPile = GameObject.Find("Green Enemies");
		GameObject redPile = GameObject.Find("Red Enemies");
		GameObject whitePile = GameObject.Find("White Enemies");
		GameObject brownPile = GameObject.Find("Brown Enemies");
		GameObject purplePile = GameObject.Find("Purple Enemies");
		GameObject greyPile = GameObject.Find("Grey Enemies");
		EnemyScript newEnemy;
		for (int i = 0; i < transform.childCount; i++){
			HexScript hex = transform.GetChild(i).GetComponent<HexScript>();
			if (hex != null){
				switch (hex.hexFeature) {
				case Toolbox.HexFeature.RampageGreen:
					if (greenPile.transform.childCount == 1){
						Manager.ShuffleEnemyStack(Toolbox.EnemyColour.Green);
					}
					newEnemy = greenPile.transform.GetChild(0).gameObject.GetComponent<EnemyScript>();
					newEnemy.SetFacing(true);
					photonView.RPC("AddHome", PhotonTargets.AllBuffered, hex.gameObject.GetPhotonView().viewID, newEnemy.gameObject.GetPhotonView().viewID);
					photonView.RPC("AddEnemy", PhotonTargets.AllBuffered, hex.gameObject.GetPhotonView().viewID, newEnemy.gameObject.GetPhotonView().viewID);
					photonView.RPC("Parenting", PhotonTargets.AllBuffered, greenPile.transform.GetChild(0).gameObject.GetPhotonView().viewID, hex.gameObject.GetPhotonView().viewID, false);
					break;
				case Toolbox.HexFeature.RampageRed:
					if (redPile.transform.childCount == 1){
						Manager.ShuffleEnemyStack(Toolbox.EnemyColour.Red);
					}
					newEnemy = redPile.transform.GetChild(0).gameObject.GetComponent<EnemyScript>();
					newEnemy.SetFacing(true);
					photonView.RPC("AddHome", PhotonTargets.AllBuffered, hex.gameObject.GetPhotonView().viewID, newEnemy.gameObject.GetPhotonView().viewID);
					photonView.RPC("AddEnemy", PhotonTargets.AllBuffered, hex.gameObject.GetPhotonView().viewID, newEnemy.gameObject.GetPhotonView().viewID);
					photonView.RPC("Parenting", PhotonTargets.AllBuffered, redPile.transform.GetChild(0).gameObject.GetPhotonView().viewID, hex.gameObject.GetPhotonView().viewID, false);
					break;
				case Toolbox.HexFeature.Base:
					if (greyPile.transform.childCount == 1){
						Manager.ShuffleEnemyStack(Toolbox.EnemyColour.Grey);
					}
					newEnemy = greyPile.transform.GetChild(0).gameObject.GetComponent<EnemyScript>();
					photonView.RPC("AddEnemy", PhotonTargets.AllBuffered, hex.gameObject.GetPhotonView().viewID, newEnemy.gameObject.GetPhotonView().viewID);
					photonView.RPC("Parenting", PhotonTargets.AllBuffered, greyPile.transform.GetChild(0).gameObject.GetPhotonView().viewID, hex.gameObject.GetPhotonView().viewID, false);
					if (Toolbox.Instance.isDay){
						newEnemy.SetFacing(true);
					}
					photonView.RPC("AddHome", PhotonTargets.AllBuffered, hex.gameObject.GetPhotonView().viewID, newEnemy.gameObject.GetPhotonView().viewID);
					newEnemy.siteFortification = true;
					break;
				case Toolbox.HexFeature.DarkMatterResearch:
					if (purplePile.transform.childCount == 1){
						Manager.ShuffleEnemyStack(Toolbox.EnemyColour.Purple);
					}
					newEnemy = purplePile.transform.GetChild(0).gameObject.GetComponent<EnemyScript>();
					photonView.RPC("AddEnemy", PhotonTargets.AllBuffered, hex.gameObject.GetPhotonView().viewID, newEnemy.gameObject.GetPhotonView().viewID);
					photonView.RPC("Parenting", PhotonTargets.AllBuffered, purplePile.transform.GetChild(0).gameObject.GetPhotonView().viewID, hex.gameObject.GetPhotonView().viewID, false);
					if (Toolbox.Instance.isDay){
						newEnemy.SetFacing(true);
					}
					photonView.RPC("AddHome", PhotonTargets.AllBuffered, hex.gameObject.GetPhotonView().viewID, newEnemy.gameObject.GetPhotonView().viewID);
					newEnemy.siteFortification = true;
					break;
				default:
					break;
				}
			}
		}
	}
}
