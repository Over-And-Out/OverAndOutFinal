var Flashlight: GameObject;
  function OnTriggerEnter(otherObj: Collider){
      Debug.Log("triggered"); // trigger works finally
      if (otherObj.tag == "Player"){
          Debug.Log("Flashlight enabled"); // Cena got enabled, yay \o/
          Flashlight.SetActive(true);
      } 
      else {
          Debug.Log("Cena disabled"); // Just to check it's not instantly disabled again
          Flashlight.SetActive(false);
      }
  }