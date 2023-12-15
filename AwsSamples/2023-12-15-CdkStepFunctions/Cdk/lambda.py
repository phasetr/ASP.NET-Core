import random


def lambda_handler(event, context):
    rand = random.randint(1, 3)  # 1から3までの乱数を生成
    if event['InputPath'] == rand:
        result = 'Tied'  # 入力値とrandが同じならあいこ
    elif event['InputPath'] == 3 and rand == 1:
        result = 'You win'  # 入力値が3のとき、randが1なら勝ち
    elif event['InputPath'] == 1 and rand == 3:
        result = 'You lose'  # 入力値が1のとき、randが3なら負け
    elif event['InputPath'] < rand:
        result = 'You win'  # 入力値よりもrandが大きければ勝ち
    elif event['InputPath'] > rand:
        result = 'You lose'  # 入力値よりもrandが小さければ負け
    else:
        result = 'Error'
    return {
        "bar": result
    }
