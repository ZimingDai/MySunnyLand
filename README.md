# SunnyLand

>感谢Ansimuz的开源项目：SunnyLand
>
>网址：https://assetstore.unity.com/packages/2d/characters/sunny-land-103349

### 其他需求



### 如何Import Asset

1. 在Unity中点击Window中的 Asset Store
2. 然后再Store中搜索需要的项目，在Unity中打开后，import



### 水平移动

要先获得这个player的rigidbody2d，这其实是一个物理模型。

然后通过getAxis来获取输入的按键，返回值-1-1；

在update中，调用move函数。

```c#
void Movement()
    {
        float horizontalMove;
        horizontalMove = Input.GetAxis("Horizontal");
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        }
    }

//跳跃也一样
if (Input.GetButtonDown("Jump"))
	{
    rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
  }
```

#### 面向方向

**区分`getAxis()`与`getAxisRaw()`**

* `getAxis()`是获得[-1, 1]的连续数
* `getAxisRaw()`是获得-1，1，0三个数



所以我们需要更改player的scale来更改方向

```c#
float faceDirection = Input.GetAxisRaw("Horizontal");//直接获得-1，0，1
if (faceDirection != 0)
	{
		transform.localScale = new Vector3(faceDirection, 1, 1);
	}
```



### Animation

在Sprite下创建Animator

然后在Assets中创建Animator Controller；

在Window中的Animation菜单中将PNG拖拽进去，然后通过更改Sample rate来更改循环速度。

之后再Animator中添加parameter和transaction，来规定动画。

#### 判断是否isGround

1. 先获得LayerMask
2. 添加Layers，Ground；然后将地面图层归到Ground
3. 之后再将Script中的LayerMask定为Ground；
4. 添加collider 变量
5. 利用`isTouchingLayer()`来判断是否碰撞

```c#
void SwitchAnim()
    {
        anim.SetBool("isIdle", false);
        if (anim.GetBool("isJumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping", false);
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("isFalling", false);
            anim.SetBool("isIdle", true);
        }
    }
```



> 如果这个Animation是每一个State都可以到达，就直接用AnyState连接
>
> trigger也是一个parameter，就是简单触发器

### 镜头

添加<kbd>Cinemachine</kbd>组件

然后添加一个2d camera，这样main camera的所有数据都会被归并到新添加的camera中。

然后通过添加一个confiner

之后再background中添加polygoncollider，来规定camera的移动距离

**注意：要把这个collider设置为isTrigger，不然player被弹飞了……**



### 标签

Tag，用来检测是否碰撞的时候使用，不需要新建layer

```c#
private void OnTriggerEnter2D(Collider2D collision)// 这个参数
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);//销毁游戏体
        }
    }
```



### Prefabs

当做好一个Sprite，就可以反向拖回到文件夹中，成为一个预置

这样下一次使用，参数都不变，**不需要进一步设置**



### Material

在Create 2D中选择 <kbd>Physical Material 2D</kbd>

然后可以更改摩擦力和弹力



### UI

**UI最重要的是Canvas**

所以先创建：UI：Canvas

之后再在Canvas中继续创建需要的UI

> Canvas会自动跟着Camera走
>
> 但是里面的UI可能会出边界，所以在UI中Rect Transform中确定其位置。

----



注意，当我们在其他的地方想让Player有其他的反弹动作，一定要注意在update中，movement会一直调用，给新的vector2，所以必须先让movement不工作，再调用函数。



### EnemyAI

Enemy的移动范围可以添加两个Empty项目，然后在左上角更改颜色使其可见。

子项目会继承父项目的移动。

解决方法：

1. 断绝继承关系。

```c#
transform.DetachChildren();
```

​	**但是：如果enemy过多就会出现很多的left和right**



2. 获得x值，然后直接销毁left和right

```c#
 void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //断绝子关系
        // transform.DetachChildren();
        leftx = leftPoint.position.x;
        rightx = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }
```



#### Event

为了让动画播完再进行下一次动画，可以在Animation中添加Event，Event可以调用函数，将Event放在你想要的帧上就可以实现在想要的地方做下一个动作。



#### Class

不同文件中的文件调用。

```c#
FrogController frog = collision.gameObject.GetComponent<FrogController>()
```

collision中的gameobject获得`frogcontroller`的所有内容

获得父集的Start

```c#
base.Start();
```

子集能改父集的代码

```c#
//父
protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }
//子
protected override void Start()
    {
        base.Start();
  			// 调用父类函数
    }
```



### Audio

* Audio Listener 每个人的耳朵
* Audio Source 音源
* Audio Clips 声音片段

> 背景音乐要放到Player中，因为镜头随着Player走。

利用Audio Source，来添加AudioClip

播放声音：

```c#
audio.Play();
```

