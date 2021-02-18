using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform spawnPoint = null;

    

    #region Server

    [Command]
    private void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(
            unitPrefab, 
            spawnPoint.position, 
            spawnPoint.rotation);

        //Necessary to spawn the object on the server, to push it to all other clients
        NetworkServer.Spawn(unitInstance, connectionToClient);  //connectionToClient from NetworkBehaviour, assigns ownership to the client
    }

    #endregion



    #region Client

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) { return; }
        if(!hasAuthority) { return; }   //only spawn the object if you've clicked on your own spawner (ie, not someone else's)

        CmdSpawnUnit();
        
    }

    #endregion
}
