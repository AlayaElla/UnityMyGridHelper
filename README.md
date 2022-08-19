# UnityMyGridHelper
My Unity Layerout Group,with tween
说明：
unity中自带的Grid Layout Group是按照子物体的顺序来进行排序，因此在UGUI中不能控制layerout中子物体的显示层级，一定是后添加的物体层级在上。
很多时候我并不想要这样，我希望后添加的物体始终在最底层。
于是只能自己做一个，并且在运行模式中添加了缓动效果，让游戏过程中的排序过程更加流畅。

脚本会自动检测子物体的添加和删除，进行实时排序，因此和自带的layeroutgroup一样只需要添加子物体进layerout即可，不需要对代码进行任何操作。
支持在编辑器模式中实时调整，可以进行实时添加删除操作。

目前只实现了GridLayerout和VerticalLayerOut。

效果如下：

**UI：**

![image](https://s2.loli.net/2022/08/03/KsJe5YUIfXkbFiz.gif)

两个layout时，可以做到这样的动画效果。

![image](https://s2.loli.net/2022/08/03/ZS6h3rEvtcw5GBR.gif)

**场景中：**

![image](https://cdn.alaya.cool/wp-content/uploads/2022/08/1660912628-Honeycam-2022-08-19-19-53-10.gif)

![image](https://user-images.githubusercontent.com/21375302/185624547-9ad35991-8d0f-4c5e-81ee-7e64bae13e46.png)
