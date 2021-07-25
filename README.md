# SunnyLand

>感谢Ansimuz的开源项目：SunnyLand
>
>网址：https://assetstore.unity.com/packages/2d/characters/sunny-land-103349

## 其他需求



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
```

