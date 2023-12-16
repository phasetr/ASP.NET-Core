import random


def lambda_handler(event, context):
    rand = random.randint(1, 3)  # 1から3までの乱数を生成
    try:
        print(event)
        # eventが整数変数かどうか確認
        inputPath = int(event) if isinstance(event, int) or isinstance(event, str) else int(event["InputPath"])
        if inputPath == rand:
            result = 'Tied'  # 入力値とrandが同じならあいこ
        elif inputPath == 3 and rand == 1:
            result = 'You win'  # 入力値が3のとき、randが1なら勝ち
        elif inputPath == 1 and rand == 3:
            result = 'You lose'  # 入力値が1のとき、randが3なら負け
        elif inputPath < rand:
            result = 'You win'  # 入力値よりもrandが大きければ勝ち
        elif inputPath > rand:
            result = 'You lose'  # 入力値よりもrandが小さければ負け
        else:
            result = 'Error'
        return {
            "bar": result
        }
    except Exception as e:
        print(e)
        return {
            "bar": "Error"
        }
