import random


def lambda_handler(event, context):
    rand = random.randint(1, 3)  # 1から3までの乱数を生成
    try:
        print(event)
        input_path = ""
        parameters = ["throw", "InputPath", "Payload"]
        if isinstance(event, int):
            input_path = int(event)
        else:
            for parameter in parameters:
                if parameter in event:
                    print(f"parameter: {parameter}")
                    input_path = int(event[parameter])
                    break
        print(f"input_path: {input_path}")
        if input_path == "":
            result = 'Invalid Input'
        elif input_path == rand:
            result = 'Tied'  # 入力値とrandが同じならあいこ
        elif input_path == 3 and rand == 1:
            result = 'You win'  # 入力値が3のとき、randが1なら勝ち
        elif input_path == 1 and rand == 3:
            result = 'You lose'  # 入力値が1のとき、randが3なら負け
        elif input_path < rand:
            result = 'You win'  # 入力値よりもrandが大きければ勝ち
        elif input_path > rand:
            result = 'You lose'  # 入力値よりもrandが小さければ負け
        else:
            result = 'Error'
        return {
            "bar": result
        }
    except Exception as e:
        print(f"Exception: {e}")
        return {
            "bar": "Error"
        }
