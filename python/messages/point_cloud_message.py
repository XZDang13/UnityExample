import json
import numpy as np

class PointCloudMessage:
    def __init__(self, point_json):
        self.point_json = point_json

    @staticmethod
    def from_json(json_string):
        point_cloud_message_dict = json.loads(json_string)
        return PointCloudMessage(point_cloud_message_dict["point_json"])

    def to_json(self):
        return json.dumps(self.__dict__)