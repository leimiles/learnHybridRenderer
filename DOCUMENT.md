### Hybrid Renderer 0.51-0

####  简介

Hybrid Renderer 提供了用来渲染 ECS 的 Entity 的条件，可以将 Entity 渲染出来，它并不是渲染管线；本质上它是一个 system，这个 system 集合了用来渲染 ECS Entities 所需要的数据，并且将这些数据发送给当前的渲染管线中

Hybrid Renderer 的作用就是将 DOTS 与当前的渲染管线逻辑关联起来，这样就可以使用 ECS Entity 来代替传统的 GameObject，从而大幅度优化运行时的内存状态与性能表现

Hybrid Renderer 包含多个 systems，能够将 GameObjects 转换成对应的 DOTS entities. 可以在 Editor 端进行转换，也可以在 runtime 下进行转换，前者则会在场景加载上表现的更好

如果需要在 editor 段转换，需要他们放在一个 subscene 中，editor 会执行转换并将结果保存在磁盘上，如果要在 runtime 时转换，则需要添加 ConvertToEntity 的组件给到要被转换的 GameObjects

转换的过程中 unity 的主要步骤如下

>* 首先，conversion system 首先会将 MeshRenderer 和 MeshFilter 的 component 转换成 DOTS RenderMesh 的 component, 根据当前不同的渲染管线，conversion system 可能还会添加一些其他的与渲染相关的 components 在 entity 上

>* 其次，conversion system 会将 LOD Group 的 components，转换为 MeshLODGroupComponents，LOD Group 关联的每一个 Entity 都会产生一个 DOTS MeshLODComponent

>* 最后，conversion system 会将每个 gameObject 的 transform 转换成 DOTS LocalToWorld component，根据 transform 不同的属性状态，conversion system 可能会添加 DOTS translation，rotation，和 nonuniformscale 这些 component

runtime 情况下

Hybrid Renderer 会处理所有带有 LocalToWorld，RenderMesh，RenderBounds components 的 entities. 许多 HDRP，URP 特性需要他们自己的 material Property components，这些 components 都是在 MeshRenderer 转换中被添加的，处理过后的 entities 会变添加到 batches 中，unity 会使用 srp batcher 来渲染每个 batch

注意：如果在 runtime 情况下像场景中添加 entities，最好是通过 instantiate prefab 的方式，而非新建一个 entity. 由于 hybrid renderer 会频繁更新，所以最好的实践方式是使用 conversion pipeline 以及 prefabs，而非手动重新创建 entities，这样做能够避免 hybrid renderer 升级带来的兼容性问题

#### 要求与兼容性

>* built-in，不支持
>* HDRP，HDRP 9.0+ 以及 unity 2020.1+
>* URP，URP 9.0+ 以及 unity 2020.1+

Hybrid Renderer 当前不支持 desktop OpenGL 以及 GLES，而移动端则必须使用 vulkan 的 API，但对于一些老旧的 android 设备，vulkan 已经放弃支持更新，在未来的版本中会支持 OpenGL 以及 GLES3.1

Hybrid Renderer 当前未在移动端与 console 端完整验证，建议不要尝试

Hybrid Renderer 目前尚不支持 XR 设备（未来可能会），以及 ray-tracing

Hybrid Renderer 不支持多重 DOTS Worlds，未来会倾向支持有限多个 DOTS Worlds，目前的计划是添加对多重 rendering system 的支持，即每个可读的 world 都单独跑一个 rendering system, 只是同一时间只有一个 world 能够 active

Hybrid Renderer V1 已经被移除，目前可用的是 Hybrid Renderer V2

目前 Hybrid Renderer 在 URP 特性上的支持包括

>* Material Property Overrides
>* Built-in Property Overrides
>* Shader Graph
>* Lit Shader
>* UnLit Shader
>* Render Layer
>* Transform Params
>* Disable Rendering
>* Sun Light
>* Ambient Probe
>* Light Probe
>* Lightmaps
>* Transparencies (sorted)
>* Occlusion culling (dynamic) 测试中
>* Skinning / mesh deform 测试中

Hybrid Renderer 在 URP 特性上的将来会支持包括

>* Point + Spot Lights
>* Reflection probes
>* LOD Crossfade
>* Viewport shader overrides

#### 添加 Hybrid Renderer

Hybrid Renderer 不能通过 package manager 添加，必须通过修改 manifest.json 文件来添加

例如: "com.unity.rendering.hybrid": "0.51.0-preview.32"，添加指定版本，同时会添加 DOTS 的依赖

URP 项目，必须启用 SRP Batcher

Hybrid Renderer 不支持 gamma space

#### Hybrid Renderer 的特性



