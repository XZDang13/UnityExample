import json
import numpy as np

class PointMessage:
    def __init__(self, position, normal):
        self.position = position
        self.normal = normal

    @staticmethod
    def from_json(json_string):
        point_message_dict = json.loads(json_string)
        return PointMessage(point_message_dict["position"], point_message_dict["normal"])

    def to_json(self):
        return json.dumps(self.__dict__)