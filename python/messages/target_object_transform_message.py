import json

class TargetObjectTransformMessage:
    def __init__(self, position, euler):
        self.position = position
        self.euler = euler
        
    @staticmethod
    def from_json(json_string):
        message_dict = json.loads(json_string)
        return TargetObjectTransformMessage(message_dict["position"], message_dict["euler"])

    def to_json(self):
        return json.dumps(self.__dict__)
