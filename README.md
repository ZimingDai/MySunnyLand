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





### 镜头

添加<kbd>Cinemachine</kbd>组件

然后添加一个2d camera，这样main camera的所有数据都会被归并到新添加的camera中。

然后通过添加一个confiner

之后再background中添加polygoncollider，来规定camera的移动距离

==注意：要把这个collider设置为isTrigger，不然player被弹飞了……==

