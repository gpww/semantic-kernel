import io
import json
import sys
import zhipuai

import silver
from silver import receive_stream_text
from silver import stream_output_text

def invoke(model, prompt, top_p, temperature):
    response = zhipuai.model_api.invoke(
        model= model,
        prompt= prompt,
        top_p= top_p,
        temperature= temperature,
        return_type = "text"
    )
    try:
        content = response['data']['choices'][0]['content']
        print(content)
    except Exception as e:
        sys.stderr.write("response解析错误：\n")
        sys.stderr.write(json.dumps(response))

def sse_invoke(model, prompt, top_p, temperature):
    response = zhipuai.model_api.sse_invoke(
        model= model,
        prompt= prompt,
        top_p= top_p,
        temperature= temperature,
        return_type = "text",
        incremental=True
    )
    for event in response.events():
        if event.event == "add":
            print(event.data)
            sys.stdout.flush()
        elif event.event == "error" or event.event == "interrupted":
            print(event.data)
        # elif event.event == "finish":
        #     print(event.data)
        #     print(event.meta)
        else:
            print(event.data)

def main(argv):

    data = json.loads(argv)

    # 读取字段
    zhipuai.api_key = data.get('api_key')
    prompt = data.get('prompt')

    func_type = data.get('func_type','invoke')
    model = data.get('model', 'chatglm_turbo')
    top_p = data.get('top_p', 1.0)
    temperature = data.get('temperature', 0.7)

    if func_type == 'invoke':
        invoke(model, prompt, top_p, temperature)
    elif func_type == 'sse_invoke':
        sse_invoke(model, prompt, top_p, temperature)

if __name__ == "__main__":
    args = receive_stream_text()
    args = args[1:]#去掉第一个诡异字符
    main(args)