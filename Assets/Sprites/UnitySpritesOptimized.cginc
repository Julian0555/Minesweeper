// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

struct appdata_t
{
    float4 vertex : POSITION;
    float4 color : COLOR;
    float2 texcoord : TEXCOORD0;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
};

v2f SpriteVert(appdata_t IN)
{
    v2f OUT;
    
    OUT.vertex = UnityObjectToClipPos(IN.vertex);
    OUT.texcoord = IN.texcoord;
    OUT.color = IN.color;

    return OUT;
}

sampler2D _MainTex;

fixed4 SampleSpriteTexture (float2 uv)
{
    fixed4 color = tex2D (_MainTex, uv);

    return color;
}

fixed4 SpriteFrag(v2f IN) : SV_Target
{
    fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
    c.rgb *= c.a;
    return c;
}
