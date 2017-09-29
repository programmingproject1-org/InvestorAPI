#!/usr/bin/env python
# -*- coding: utf-8 -*-

import requests
import json
from pprint import pprint

WATCHLIST_MODEL = {
	"id": {"key_only": True, "is_collection": False},
	"name": {"key_only": False, "is_collection": False, "value": "Default Watchlist"}
}

RESPONSE_MODEL = {
	"key_only": False,
	"is_collection": True,
	"model": {
		"id": {"key_only": True, "is_collection": False},
		"displayName": {"key_only": False, "is_collection": False, "value": "John Doe"},
		"watchlists": {"key_only": False, "is_collection": True, "model": WATCHLIST_MODEL}
	}
}

RESPONSE_MODEL = {
	"key_only": False,
	"is_collection": True,
	"model": {
		"symbol": {"key_only": False, "is_collection": False, "value": "MRC"},
		"ask": {"key_only": True, "is_collection": False},
		"askSize": {"key_only": True, "is_collection": False},
		"bid": {"key_only": True, "is_collection": False},
		"bidSize": {"key_only": True, "is_collection": False},
		"last": {"key_only": True, "is_collection": False},
		"lastSize": {"key_only": True, "is_collection": False},
		"change": {"key_only": True, "is_collection": False},
		"changePercent": {"key_only": True, "is_collection": False},
		"dayLow": {"key_only": True, "is_collection": False},
		"dayHigh": {"key_only": True, "is_collection": False}
	}
}

SERVER_RESPONSE = [
	{
		"symbol": "ANZ",
		"ask": 29.62,
		"bid": 29.59,
		"bidSize": 1430000,
		"last": 29.6,
		"lastSize": 3029,
		"change": -0.15,
		"changePercent": -0.5,
		"dayLow": 29.46,
		"dayHigh": 29.89
	},
	{
		"symbol": "MRC",
		"ask": 29.62,
		"askSize": 746700,
		"bid": 29.59,
		"last": 29.6,
		"lastSize": 3029,
		"change": -0.15,
		"changePercent": -0.5,
		"dayLow": 29.46,
		"dayHigh": 29.89
	},
	{
		"symbol": "BBB",
		"ask": 29.62,
		"askSize": 746700,
		"bid": 29.59,
		"bidSize": 1430000,
		"last": 29.6,
		"lastSize": 3029,
		"change": -0.15,
		"changePercent": -0.5,
		"dayLow": 29.46,
		"dayHigh": 29.89
	}
]

USER_RESPONSE = [{
	"id": "#1",
	"email": "user1@test.com",
	"displayName": "User 1",
	"level": "Investor",
	"accounts": [
		{
			"id": "User 1, account 1",
			"name": "Default Account",
			"balance": 1000000
		}
	],
	"watchlists": [
		{
			# "id": "User 1, watchlist 1",
			"name": "User 1, watchlist 1; #name"
		},
		{
			"id": "User 1, watchlist 2",
			"name": "User 1, watchlist 2; #name"
		},
		{
			"id": "User 1, watchlist 3",
			"name": "User 1, watchlist 3; #name"
		}
	]
},
{
	"id": "#2",
	"email": "user2@test.com",
	"displayName": "User 2",
	"level": "Investor",
	"accounts": [
		{
			"id": "User 2, account 1",
			"name": "Default Account",
			"balance": 1000000
		}
	],
	"watchlists": [
		{
			"id": "User 1, watchlist 1",
			"name": "Default Watchlist"
		}
	]
}
]

class ResponseValidator:
	def __init__(self, server_response, acceptable_response_code, acceptable_response_model):
		self.acceptable_response_code = acceptable_response_code
		self.acceptable_response_model = acceptable_response_model
		self.server_response_code = server_response.status_code
		try:
			self.server_response_body = server_response.json()
		except ValueError:
			self.server_response_body = None

		self.errors = []

	def validate_response_code(self, server_response_code, acceptable_response_code):
		if server_response_code == acceptable_response_code:
			passed = True
		else:
			passed = False
			self.errors.append("Expected HTTP {0}; got HTTP {1}".format(acceptable_response_code, server_response_code))
		return passed

	def validate_response_body_driver(self, server_json, acceptable_model):
		if server_json is None: return False
		body_errors = 0
		if acceptable_model["is_collection"]:
			if "accept_empty" in acceptable_model:
				if not acceptable_model["accept_empty"] and len(server_json) == 0:
					body_errors += 1
					self.errors.append("Expected item in collection but got none")
					return False
			for item in server_json:
				for key in acceptable_model["model"]:
					if key not in item:
						body_errors += 1
						self.errors.append("Missing key [{0}]".format(key))
					else:
						if not acceptable_model["model"][key]["key_only"]:
							self.validate_response_body_driver(item[key], acceptable_model["model"][key])
		else:
			# several values accepted
			if isinstance(acceptable_model["value"], list):
				if server_json not in acceptable_model["value"]:
					body_errors += 1
					self.errors.append("Expected value: [{0}]; Received value: [{1}] instead".format(acceptable_model["value"], server_json))
			elif server_json != acceptable_model["value"]:
				body_errors += 1
				self.errors.append("Expected value: [{0}]; Received value: [{1}] instead".format(acceptable_model["value"], server_json))

		if body_errors != 0:
			return False

		return True

	def response_code_success(self):
		return self.validate_response_code(self.server_response_code, self.acceptable_response_code)

	def response_body_success(self): 
		return self.validate_response_body_driver(self.server_response_body, self.acceptable_response_model)

	def get_errors(self):
		return self.errors





# class ResponseValidator:
# 	def __init__(self, server_response, acceptable_response_code, acceptable_keys_values, 
# 		check_keys_only = False):
# 		self.acceptable_response_code = acceptable_response_code
# 		self.acceptable_keys_values = acceptable_keys_values
# 		self.check_keys_only = check_keys_only
# 		self.server_response_code = server_response.status_code
# 		try:
# 			self.server_response_body = server_response.json()
# 		except ValueError:
# 			self.server_response_body = None

# 		self.errors = []

# 	def validate_response_code(self, server_response_code, acceptable_response_code):
# 		if server_response_code == acceptable_response_code:
# 			passed = True
# 		else:
# 			passed = False
# 			self.errors.append("Expected HTTP {0}; got HTTP {1}".format(acceptable_response_code, server_response_code))

# 		return passed

# 	def validate_response_body(self, server_response_body, acceptable_keys_values, is_collection = False, check_keys_only = False):
		
# 		# if is_collection:
# 		# 	for item in server_response_body:
# 		# 		if isinstance(item, list): 
# 		# 			is_collection = True
# 		# 		else:
# 		# 			is_collection = False
# 		# 		validate_response_body(server_response_body, acceptable_keys_values, is_collection, check_keys_only)

# 		error_count = 0
# 		if acceptable_keys_values is None:
# 			return True
# 		elif server_response_body is None:
# 			self.errors.append("Response body is empty")
# 			error_count += 1
# 		else:
# 			for key in acceptable_keys_values:
# 				if key not in server_response_body:
# 					self.errors.append("Missing key {0}".format(key))
# 					error_count += 1
# 				elif not check_keys_only:
# 					if isinstance(acceptable_keys_values[key], list):
# 						if server_response_body[key] not in acceptable_keys_values[key]:
# 							self.errors.append("Incorrect value for key {0}; expected {1}; got {2}".format(key, " | ".join(acceptable_keys_values[key]), server_response_body[key]))
# 							error_count += 1
# 					elif acceptable_keys_values[key] != server_response_body[key]:
# 						self.errors.append("Incorrect value for key {0}; expected {1}; got {2}".format(key, acceptable_keys_values[key], server_response_body[key]))
# 						error_count += 1

# 		if error_count != 0:
# 			return False
# 		else:
# 			return True

# 	def response_code_success(self):
# 		return self.validate_response_code(self.server_response_code, self.acceptable_response_code)

# 	def response_body_success(self):
# 		return self.validate_response_body(self.server_response_body, self.acceptable_keys_values, self.check_keys_only)

# 	def get_errors(self):
# 		return self.errors

if __name__ == "__main__":
	validator = ResponseValidator(SERVER_RESPONSE, 200, RESPONSE_MODEL)
	is_success = validator.response_body_success()
	print(is_success)
	pprint(validator.get_errors())