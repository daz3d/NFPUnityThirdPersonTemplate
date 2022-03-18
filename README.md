# NFPUnity3rdPersonTemplate
This provides some code and assets that can get you started using your NFP in a 3rd Person style game quickly.

More about Non-Fungible People (NFP):
They are 8,888 beautifully rendered women and non-binary avatars algorithmically generated using the leading character generation tech on the market, with proof of ownership stored on the Ethereum blockchain.
When you own an NFP, you can download 3d assets for that character, for use in games and other experiences.

More about using this NFP 3rd Person Template:
Pull down the branch, and copy into an existing Unity project, or download the provided Unity Asset Package and import it using this option from within Unity: Assets->Import Package->Custom Package... and selecting the NFP3rdPersonTemplate.unitypackage package.

What this template provides:
* A character controller set up for two animations: An Idle animation and a Walk animation.  One source of animations is to use Mixamo.  To see how to use Mixamo for Unity, watch this video: https://youtu.be/-FhvQDqmgmU.  Once you have selected an Idle and Walk animation, attach their Animation Clips to the CharacterController provided in this package, in the Movement state blend tree.
* Two prefabs, which use a Cinemachine 3rd person camera to have a nice camera to start with.  
* The First prefab is NFPMainCamera, which can be dragged into the scene to act as the Main Camera.  You should make sure you don't have any other camera in the scene to start with.
* The Second prefab is NFPCharacter, which is designed to be the root game object for your NFP.  It has a script attached to it that handles standard 3rd person movement (Mouse and WASD plus space to jump).  It also allows for you to right click to rotate around your character to better view it in the scene.  This prefab should be placed where you want your character to start in the scene.
* Once these two prefabs are in the scene, you'll need to hook up a couple script values:
  *   On the NFPMainCamera game object, scroll to the "Look With Mouse" script, and connect the Player Body property to your NFPCharacter game object instance.
  *   On the NFPMainCamera->FollowCamera game object, hook the CinemachineVirtualCamera Follow property to your NFPCharacter->FollowTarget game object.
* Finally, in the Scripts folder is the NFPPlayerMovement script.  It will keep the Character Controller Speed parameter updated so that the proper animation is played, and controls the 3rd person camera rotation when you hold the right mouse button, as well as handling the player movement.
* Once this is all set up, you will want a NFP to use.  The best means right now is to install the NFP into Daz Studio as described in the NFP discord 
